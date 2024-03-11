using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labs
{    
    public class ArrayCreator
    {
        public void GenerateArrayA(ref int[] a, ref double[] b, int n)
        {
            a = new int[n];
            b = new double[n];
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                a[i] = rnd.Next(20000);
            }
        }

        public double[] GenerateArrayB(int[] a, double[] b, int start, int end)
        {            

            for (int i = start; i < end; i++)
            {
                double sum = 0;

                for (int j = 0; j < end; j++)
                {
                    sum += Math.Pow(a[i], 1.789);
                }

                b[i] = sum;
            }

            return b;
        }

        //public void GenerateArayBParallel(int[] a, double[] b, ref int threadOffsetStart, int threadsAmount)
        //{
        //    int n = a.Length;
        //    List<Thread> threads = new List<Thread>();
        //    for (int i = 0; i < threadsAmount; i++)
        //    {
        //        int threadOffset = threadOffsetStart;
        //        int threadOffsetEnd = threadOffset + n / threadsAmount;
        //        threadOffsetStart = threadOffsetEnd;
        //        Thread thread = new Thread(() => GenerateArrayB(a, b, threadOffset, threadOffsetEnd));
        //        threads.Add(thread);
        //        thread.Start();
        //    }

        //    foreach (Thread thread in threads)
        //    {
        //        thread.Join();
        //    }
        //}

        public void GenerateArayBParallel(int[] a, double[] b, int threadsAmount)
        {
            int n = a.Length;
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < threadsAmount; i++)
            {
                int threadOffset = i * (n / threadsAmount);
                int threadOffsetEnd = 0;
                if (i % 2 == 0)
                {
                     threadOffsetEnd = (i + 2) * (n / threadsAmount);                    
                }   
                else
                {
                    threadOffsetEnd = (i + 1) * (n / threadsAmount) / 2;
                }

                tasks.Add(Task.Run(() => GenerateArrayB(a, b, threadOffset, threadOffsetEnd)));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
