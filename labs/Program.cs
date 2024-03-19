using System.Diagnostics;

class Program
{
    static double Function(double x)
    {
        return Math.Pow(Math.E, x);
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
            //Console.WriteLine("Result: {0}, Epsilon: {1}, Time: {2}", currentResult, epsilon, Stopwatch.GetTimestamp());
            if (Math.Abs(currentResult - prevResult) < epsilon)
                break;
            
            prevResult = currentResult;
            /*n *= 2;*/
            h /= 2;
        }

        result = prevResult;

        return result;
    }

    /*static double ParallelIntegration(double a, double b, double epsilon, int numThreads)
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
                    var t = 5 * 5;
                    double threadResult = 0;
                    for (var x = threadA; x < threadB; x += step)
                    {
                        threadResult += Function(x) * step;
                    }

                    result += threadResult;
                });

                listThread.Add(th);
            }
            foreach (var th in listThread)
            {
                th.Start();
                th.Join();
            } 
                
            if (Math.Abs(result - prevResult) < epsilon)
                break;


            prevResult = result;
            step /= 2;
        }


        return result;
    }*/

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
            var threadResults = new double[numThreads];
            for (var i = 0; i < numThreads; i++)
            {
                var threadA = a + i * subStep;
                var threadB = a + (i + 1) * subStep;
                var th = new Thread((object indexObj) =>
                {
                    var t = 5 * 5;
                    int index = (int)indexObj;
                    double threadResult = 0;
                    for (var x = threadA; x < threadB; x += step)
                    {
                        threadResult += Function(x) * step;
                    }
                    threadResults[index] = threadResult; 
                });

                listThread.Add(th);
            }
                
            for (var i = 0; i < numThreads; i++)
            {
                listThread[i].Start(i);
            }

            foreach (var th in listThread)
            {
                th.Join();
            }

            for (int i = 0; i < numThreads; i++)
            {
                result += threadResults[i];
            }

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
        double a = 0;
        double b = 10;
        double epsilon = 0.001;
        double starEpsilon = 0.1;
        int maxThreads = 10;

        Console.WriteLine("Sequential Integration:");
        for (double i = starEpsilon; i >= epsilon; i /=10)
        {
            Stopwatch sw = Stopwatch.StartNew();
            double result = SequentialIntegration(a, b, i);
            sw.Stop();
            Console.WriteLine($"Epsilon: {i.ToString("F10")}, Result: {result}, Time: {sw.ElapsedMilliseconds} ms");
        }

        Console.WriteLine("\nParallel Integration:");
        for (double i = starEpsilon; i >= epsilon; i /=10)
        {
            Console.WriteLine("Epsilon: " + i.ToString("F10"));
            for (int j = 4; j <= maxThreads; j++)
            {
                double result = 0;
                Stopwatch sw = Stopwatch.StartNew();
                result = ParallelIntegration(a, b, i, j);

                sw.Stop();
                
                Console.WriteLine($"Threads: {j}, Result: {result}, Time: {sw.ElapsedMilliseconds} ms");
            }
            Console.WriteLine();
        }
    }
}
