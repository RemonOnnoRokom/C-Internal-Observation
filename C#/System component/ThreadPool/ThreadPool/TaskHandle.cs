using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public enum TaskState
    {
        Pending = 1,
        InProgress,
        Completed,
        Canceled,
        Failed
    }

    public class TaskHandle
    {
        public ClientHandle Token { get; set; }              // generate this everytime an usertask is queued and return to the caller as a reference. 
        public UserTask Task { get; set; }                   // the item to be queued - supplied by the caller
        public Action<TaskStatus> Callback { get; set; }     // optional - in case user want's a notification of completion

        public TaskHandle()
        {
            Callback = null;
            Task = null;
        }
    }
}
