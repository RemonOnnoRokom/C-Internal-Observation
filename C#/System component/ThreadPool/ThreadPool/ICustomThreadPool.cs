using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public interface ICustomThreadPool
    {
        public ClientHandle QueueUserTask(UserTask task, Action<TaskStatus> callback);
    }
}
