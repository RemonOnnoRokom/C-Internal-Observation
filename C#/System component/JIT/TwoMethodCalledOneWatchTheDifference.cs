using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIT
{
    public class TwoMethodCalledOneWatchTheDifference
    {
        private void MethodA()
        {
            int sum = 0;
            for (int i = 0; i < 1_000_000; i++)
                sum += i;
        }
        private void MethodB()
        {
            int product = 1;
            for (int i = 1; i <= 10; i++)
                product *= i;
        }
        public void CalledOne()
        {
            //            👉 এখানে:

            //            MethodA() → JIT হবে

            //              MethodB() → JIT হবেই না

            //              🔥 Core realization:

            //              JIT is lazy — only compiles what is executed

            MethodA();
        }
    }
}
