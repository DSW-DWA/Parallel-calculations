using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static double Function(double x)
    {
        return 8 + 2*x - (x*x);
    }

    static double SequentialIntegration(double a, double b, double epsilon)
    {
        double result = 0;
        double h = (b - a) / 2;
        double prevResult = double.MaxValue;
        /*int n = 1;*/

        while (true)
        {
            double sum = 0;

            for (double i = a; i <= b; i+=h)
            {
                sum += Function(i)*h;
            }

            double currentResult = sum;

            if (Math.Abs(currentResult - prevResult) < epsilon)
                break;

            prevResult = currentResult;
            /*n *= 2;*/
            h /= 2;
        }

        result = prevResult;

        return result;
    }

    static double ParallelIntegration(double a, double b, double epsilon, int numThreads)
    {
        double result = 0;
        double subStep = (b - a) / numThreads;
        double prevResult = double.MaxValue;
        var step = subStep / 2;

        while (true)
        {
            result = 0;
            var listThread = new List<Thread>();
            for (var i = 0; i < numThreads; i++)
            {
                var threadA = a + i * subStep;
                var threadB = a + (i + 1) * subStep;

                var th = new Thread(() =>
                {
                    double threadResult = 0;
                    for (var x = threadA; x < threadB; x += step)
                    {
                        threadResult += Function(x) * step;
                    }

                    result += threadResult;
                });

                th.Start();

                listThread.Add(th);
            }

            foreach (var th in listThread)
                th.Join();

            if (Math.Abs(result - prevResult) < epsilon)
                break;

            prevResult = result;
            step /= 2;
        }
        

        return result;
    }


    static void Main(string[] args)
    {
        var t = 5 * 5;
        double a = -2;
        double b = 4;
        double epsilon = 0.000000001;
        double starEpsilon = 0.001;
        int maxThreads = 8;

        Console.WriteLine("Sequential Integration:");
        for (double i = starEpsilon; i >= epsilon; i /=10)
        {
            Stopwatch sw = Stopwatch.StartNew();
            double result = SequentialIntegration(a, b, epsilon);
            sw.Stop();
            Console.WriteLine($"Epsilon: {i.ToString("F10")}, Result: {result}, Time: {sw.ElapsedTicks} ticks");
        }

        Console.WriteLine("\nParallel Integration:");
        for (double i = starEpsilon; i >= epsilon; i /=10)
        {
            Console.WriteLine("Epsilon: " + i.ToString("F10"));
            for (int j = 1; j <= maxThreads; j++)
            {
                long sum = 0;
                double result = 0;
                for (var k = 0; k < 10; k ++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    result = ParallelIntegration(a, b, i, j);

                    sw.Stop();
                    sum += sw.ElapsedTicks;
                }
                
                Console.WriteLine($"Threads: {j}, Result: {result}, Time: {sum/10} ticks");
            }
            Console.WriteLine();
        }
    }
}
