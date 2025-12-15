using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIT
{
    public class CalledTwiceWatchTheDifference
    {
        private  void Test()
        {
            int sum = 0;
            for (int i = 0; i < 1_000_000; i++)
                sum += i;
        }

        public void CalledTwiceTest()
        {
            Stopwatch sw = new Stopwatch();
            #region 1st Call Test() (JIT Compilation)
            sw.Start();
            Test();
            sw.Stop();
            Console.WriteLine("First call: " + sw.ElapsedTicks);
            #endregion

            #region 2nd Call Test() (Already JITed)
            sw.Restart();
            Test();
            sw.Stop();
            Console.WriteLine("Second call: " + sw.ElapsedTicks);
            #endregion
        }
    }
}
