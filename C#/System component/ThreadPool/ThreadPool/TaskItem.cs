using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class TaskItem // running items in the pool - TaskHandle gets a thread to execute it 
    {
        public TaskHandle taskHandle;
        public Thread handler;
        public TaskState taskState = TaskState.Pending;
        public DateTime startTime = DateTime.MaxValue;
    }
}
