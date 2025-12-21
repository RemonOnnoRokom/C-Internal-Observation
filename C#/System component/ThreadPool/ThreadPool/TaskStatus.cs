using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public delegate void UserTask();
    public class TaskStatus
    {
        public bool Success = true;
        public Exception InnerException = null;
    }
}
