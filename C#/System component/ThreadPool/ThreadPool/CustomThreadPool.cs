using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class CustomThreadPool
    {
        //#region configurable items - for demo let's have these as constants
        private const int MAX = 8; // maximum no of threads in pool
        private const int MIN = 3; // minimum no of threads in pool
        private const int MIN_WAIT = 10; // milliseconds
        private const int MAX_WAIT = 15000; // milliseconds - threshold for simple task
        private const int CLEANUP_INTERVAL = 60000; // millisecond - to free waiting threads in pool
        private const int SCHEDULING_INTERVAL = 10; // millisecond - look for task in queue in loop
                                                    //#endregion

        //#region singleton instance of threadpool
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
        //#endregion

        private void InitializeThreadPool()
        {
            //TODO: write initialization code here 
        }
    }
}
