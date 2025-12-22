using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class TaskItem // running items in the pool - TaskHandle gets a thread to execute it 
    {
        public TaskHandle TaskHandle { get; set; }
        public Thread Handler { get; set; }
        public TaskState TaskState { get; set; } 
        public DateTime StartTime { get; set; } 

        public TaskItem()
        {
            TaskState = TaskState.Pending;
            StartTime = DateTime.MaxValue;
        }
    }
}
