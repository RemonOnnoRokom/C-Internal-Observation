using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace JIT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CalledTwiceWatchTheDifference jit = new();
            jit.CalledTwiceTest();

            //TwoMethodCalledOneWatchTheDifference jit2 = new();
            //jit2.CalledOne();

            //BenchmarkRunner.Run<BenchMark>();

            //BenchMark jit3 = new();
            //jit3.WatchTheBenchMark();

            MethodJITedBefore methodJITedBefore = new();
            methodJITedBefore.CalledMethodJITedBefore();
        }
    }
}
