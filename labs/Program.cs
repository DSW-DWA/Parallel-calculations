using BenchmarkDotNet.Running;
using labs;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<ArrayBenchmark>();
    }    
}
