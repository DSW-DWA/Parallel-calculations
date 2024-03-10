using labs;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {        
        int testsCount = 8;
        int count = 5;
        List<List<int>> list = new List<List<int>>();
        Stopwatch stopWatch = new Stopwatch();
        Generator generator = new Generator(2000);

        for (int i = 0; i < testsCount; i++)
        {
            list.Add(new List<int>());
        }

        for (int i = 0; i < count; i++)
        {
            generator.CreateTransposeMatrix(generator.MatrixB, "B");
            stopWatch.Start();
            CalculateMatrixMultiplication(200, "matrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
            //Console.WriteLine("Время выполнения row by column: " + stopWatch.Elapsed.Milliseconds);
            list[0].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();

            stopWatch.Start();
            generator.CreateTransposeMatrix(generator.TransposedMatrix, "T");
            CalculateMatrixMultiplication(200, "matrixA.bin", "transposedMatrixT.bin", "resultRowColumn.bin");
            // Console.WriteLine("Время выполнения row by column(T): " + stopWatch.Elapsed.Milliseconds);
            list[1].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();

            stopWatch.Start();
            CalculateMatrixMultiplication(200, "matrixA.bin", "matrixB.bin", "resultRowColumn.bin");
            //Console.WriteLine("Время выполнения row by row: " + stopWatch.Elapsed.Milliseconds);
            list[2].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();

            stopWatch.Start();
            generator.CreateTransposeMatrix(generator.MatrixB, "B");
            CalculateMatrixMultiplication(200, "matrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
            //Console.WriteLine("Время выполнения row by row(T): " + stopWatch.Elapsed.Milliseconds);
            list[3].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();

            generator.CreateTransposeMatrix(generator.MatrixA, "A");
            generator.CreateTransposeMatrix(generator.MatrixB, "B");
            stopWatch.Start();
            CalculateMatrixMultiplication(200, "transposedMatrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
            //Console.WriteLine("Время выполнения column by column: " + stopWatch.Elapsed.Milliseconds);
            list[4].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();

            stopWatch.Start();
            generator.CreateTransposeMatrix(generator.TransposedMatrix, "T");
            CalculateMatrixMultiplication(200, "transposedMatrixA.bin", "transposedMatrixT.bin", "resultRowColumn.bin");
            //Console.WriteLine("Время выполнения column by column(T): " + stopWatch.Elapsed.Milliseconds);
            list[5].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();

            stopWatch.Start();
            CalculateMatrixMultiplication(200, "transposedMatrixA.bin", "matrixB.bin", "resultRowColumn.bin");
            //Console.WriteLine("Время выполнения column by row: " + stopWatch.Elapsed.Milliseconds);
            list[6].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();

            stopWatch.Start();
            generator.CreateTransposeMatrix(generator.MatrixB, "B");
            CalculateMatrixMultiplication(200, "transposedMatrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
            //Console.WriteLine("Время выполнения column by row(T): " + stopWatch.Elapsed.Milliseconds);
            list[7].Add(stopWatch.Elapsed.Milliseconds);
            stopWatch.Reset();
        }

        Console.WriteLine("Среднее время выполнения row by column: " + list[0].Average());
        Console.WriteLine("Среднее время выполнения row by column(T): " + list[1].Average());
        Console.WriteLine("Среднее время выполнения row by row: " + list[2].Average());
        Console.WriteLine("Среднее время выполнения row by row(T): " + list[3].Average());
        Console.WriteLine("Среднее время выполнения column by column: " + list[4].Average());
        Console.WriteLine("Среднее время выполнения column by column(T): " + list[5].Average());
        Console.WriteLine("Среднее время выполнения column by row: " + list[6].Average());
        Console.WriteLine("Среднее время выполнения column by row(T): " + list[7].Average());

    }

    static void CalculateMatrixMultiplication(int matrixLength, string matrixAFilePath, string matrixBFilePath, string resultFilePath)
    {
        int N = matrixLength;

        using (BinaryReader matrixAReader = new BinaryReader(File.Open(matrixAFilePath, FileMode.Open)))
        using (BinaryReader matrixBReader = new BinaryReader(File.Open(matrixBFilePath, FileMode.Open)))
        using (BinaryWriter resultWriter = new BinaryWriter(File.Open(resultFilePath, FileMode.Create)))
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    int sum = 0;
                    matrixAReader.BaseStream.Seek(i * N * sizeof(int), SeekOrigin.Begin);
                    matrixBReader.BaseStream.Seek(j * sizeof(int), SeekOrigin.Begin);
                    for (int k = 0; k < N; k++)
                    {
                        int a = matrixAReader.ReadInt32();
                        int b = matrixBReader.ReadInt32();
                        sum += a * b;
                    }
                    resultWriter.Write(sum);
                }
            }
        }
    }
    static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
