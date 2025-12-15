using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace JIT
{
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 1, iterationCount: 5)]
    public class BenchMark
    {
        [Benchmark]
        public void WatchTheBenchMark()
        {
            CalledTwiceWatchTheDifference jit = new();
            jit.CalledTwiceTest();
        }
    }
}
