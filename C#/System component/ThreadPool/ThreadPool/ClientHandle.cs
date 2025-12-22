using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class ClientHandle
    {
        public Guid ID { get; set; }
        public bool IsSimpleTask { get; set; }

        public ClientHandle()
        {
            ID = Guid.NewGuid();
            IsSimpleTask = true;
        }
    }
}
