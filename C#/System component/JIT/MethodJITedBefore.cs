using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JIT
{
    public class MethodJITedBefore
    {
        public void Test()
        {
            int sum = 0;
            for (int i = 0; i < 1_000_000; i++)
                sum += i;
        }

        public void CalledMethodJITedBefore()
        {
            Stopwatch stw = new Stopwatch();

            #region Forcely JITed Before Call
            RuntimeHelpers.PrepareMethod(typeof(MethodJITedBefore)
            .GetMethod("Test", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)!
            .MethodHandle);
            #endregion

            Console.WriteLine("Method JITed before call");
            stw.Start();
            Test();
            stw.Stop();

            Console.WriteLine("Elapsed Ticks: " + stw.ElapsedTicks);
        }
    }
}
