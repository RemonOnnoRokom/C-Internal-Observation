using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class CustomThreadPool : ICustomThreadPool
    {
        #region configurable items - for demo let's have these as constants
        private const int MAX = 8;                    // maximum no of threads in pool
        private const int MIN = 3;                    // minimum no of threads in pool
        private const int MIN_WAIT = 10;             // milliseconds
        private const int MAX_WAIT = 15000;          // milliseconds - threshold for simple task
        private const int CLEANUP_INTERVAL = 60000; // millisecond - to free waiting threads in pool
        private const int SCHEDULING_INTERVAL = 10; // millisecond - look for task in queue in loop
        private Queue<TaskHandle> ReadyQueue = null;
        private List<TaskItem> Pool = null;
        private Thread taskScheduler = null;
        #endregion

        #region singleton instance of threadpool
        private static readonly CustomThreadPool _instance = new CustomThreadPool();

        private CustomThreadPool()
        {
            InitializeThreadPool();
        }

        public static CustomThreadPool Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        private void InitializeThreadPool()
        {
            ReadyQueue = new Queue<TaskHandle>();
            Pool = new List<TaskItem>();
            InitPoolWithMinCapacity(); // initialize Pool with Minimum capacity - that much thread must be kept ready

            DateTime LastCleanup = DateTime.Now; // monitor this time for next cleaning activity

            taskScheduler = new Thread(() =>
            {
                do
                {
                    while (ReadyQueue.Count > 0 && ReadyQueue.Peek().task == null)
                        ReadyQueue.Dequeue();
                    // remove cancelled item/s - cancelled item will have it's task set to null

                    int itemCount = ReadyQueue.Count;
                    for (int i = 0; i < itemCount; i++)
                    {
                        TaskHandle readyItem = ReadyQueue.Peek(); // the Top item of queue
                        bool Added = false;

                        foreach (TaskItem ti in Pool)
                        {
                            if (ti.taskState == TaskState.Completed)
                            {
                                // if in the Pool task state is completed then a different
                                // task can be handed over to that thread
                                ti.taskHandle = readyItem;
                                ti.taskState = TaskState.Pending;
                                Added = true;
                                ReadyQueue.Dequeue();
                                break;
                            }
                        }
                        if (!Added && Pool.Count < MAX)
                        {
                            // if all threads in pool are busy and the count is still less than the
                            // Max limit set then create a new thread and add that to pool
                            TaskItem ti = new TaskItem() { taskState = TaskState.Pending };
                            ti.taskHandle = readyItem;
                            // add a new TaskItem in the pool
                            AddTaskToPool(ti);
                            Added = true;
                            ReadyQueue.Dequeue();
                        }
                        if (!Added) break; // It's already crowded so try after sometime
                    }
                    if ((DateTime.Now - LastCleanup) > TimeSpan.FromMilliseconds(CLEANUP_INTERVAL))
                    // It's long time - so try to cleanup Pool once.
                    {
                        CleanupPool();
                        LastCleanup = DateTime.Now;
                    }
                    else
                    {
                        // either of these two can work - the combination is also fine for our demo. 
                        Thread.Yield();
                        Thread.Sleep(SCHEDULING_INTERVAL); // Dont run madly in a loop - wait for sometime for things to change.
                                                           // the wait should be minimal - close to zero
                    }
                } while (true);
            });
            taskScheduler.Priority = ThreadPriority.AboveNormal;
            taskScheduler.Start();

        }

        private void InitPoolWithMinCapacity()
        {
            for (int i = 0; i <= MIN; i++)
            {
                TaskItem ti = new TaskItem() { taskState = TaskState.Pending };
                ti.taskHandle = new TaskHandle() { task = () => { } };
                ti.taskHandle.callback = (taskStatus) => { };
                ti.taskHandle.Token = new ClientHandle() { ID = Guid.NewGuid() };
                AddTaskToPool(ti);
            }
        }

        private void AddTaskToPool(TaskItem taskItem)
        {
            taskItem.handler = new Thread(() =>
            {
                do
                {
                    bool Enter = false;

                    // if aborted then allow it to exit the loop so that it can complete and free-up thread resource.
                    // this state means it has been removed from Pool already.
                    if (taskItem.taskState == TaskState.Canceled) break;

                    if (taskItem.taskState == TaskState.Pending)
                    {
                        taskItem.taskState = TaskState.InProgress;
                        taskItem.startTime = DateTime.Now;
                        Enter = true;
                    }
                    if (Enter)
                    {
                        TaskStatus taskStatus = new TaskStatus();
                        try
                        {
                            taskItem.taskHandle.task.Invoke(); // execute the UserTask
                            taskStatus.Success = true;
                        }
                        catch (Exception ex)
                        {
                            taskStatus.Success = false;
                            taskStatus.InnerException = ex;
                        }
                        if (taskItem.taskHandle.callback != null && taskItem.taskState != TaskState.Canceled)
                        {
                            try
                            {
                                taskItem.taskState = TaskState.Completed;
                                taskItem.startTime = DateTime.MaxValue;

                                taskItem.taskHandle.callback(taskStatus); // notify callback with task-status
                            }
                            catch
                            {

                            }
                        }
                    }
                    // give other thread a chance to execute as it's current execution completed already
                    Thread.Yield(); Thread.Sleep(MIN_WAIT); //TODO: need to see if Sleep is required here
                } while (true); // it's a continuous loop until task gets abort request
            });
            taskItem.handler.Start();
            Pool.Add(taskItem);
        }

        private void CleanupPool()
        {
            throw new NotImplementedException();
        }

        #region public interface
        public ClientHandle QueueUserTask(UserTask task, Action<TaskStatus> callback)
        {
            TaskHandle th = new TaskHandle()
            {
                task = task,
                Token = new ClientHandle()
                {
                    ID = Guid.NewGuid()
                },
                callback = callback
            };
            ReadyQueue.Enqueue(th);
            return th.Token;
        }

        public static void CancelUserTask(ClientHandle handle)
        {
            //TODO: write implementation code here
        }
        #endregion
    }
}