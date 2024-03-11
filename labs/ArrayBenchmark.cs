using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace labs
{
    [MemoryDiagnoser]
    //[SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 15, iterationCount: 30, id: "FastAndDirtyJob")]
    public class ArrayBenchmark
    {
        private ArrayCreator Creator = new ArrayCreator();
        public int[] a;
        public double[] b;
        [Params(1000, 5000, 10000, 2000)]
        public int n;

        [Params(2, 4, 8, 12, 16, 20)]
        public int threadsAmount;

        //[Benchmark]
        //public void GenerateArrayBBenchmarkLinear()
        //{
        //    Creator.GenerateArrayB(a, b, 0, n);
        //}

        [GlobalSetup]
        public void Setup()
        {
            Creator.GenerateArrayA(ref a, ref b, n);
        }

        [Benchmark]
        public void GenerateArrayBBenchmarkParallel()
        {
            Creator.GenerateArayBParallel(a, b, threadsAmount);
        }
    }
}
