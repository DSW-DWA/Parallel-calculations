using labs;
using System;
using System.IO;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        var test = 5 * 5;

        Generator generator = new Generator(100);

        generator.CreateTransposeMatrix(generator.MatrixB,"B");

        var timeStar = DateTime.Now;
        CalculateMatrixMultiplication(100, "matrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
        var timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения row by column: " + (timeEnd - timeStar).Ticks);

        timeStar = DateTime.Now;
        generator.CreateTransposeMatrix(generator.TransposedMatrix, "T");
        CalculateMatrixMultiplication(100, "matrixA.bin", "transposedMatrixT.bin", "resultRowColumn.bin");
        timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения row by column(T): " + (timeEnd - timeStar).Ticks);

        timeStar = DateTime.Now;
        CalculateMatrixMultiplication(100, "matrixA.bin", "matrixB.bin", "resultRowColumn.bin");
        timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения row by row: " + (timeEnd - timeStar).Ticks);

        timeStar = DateTime.Now;
        generator.CreateTransposeMatrix(generator.MatrixB, "B");
        CalculateMatrixMultiplication(100, "matrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
        timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения row by row(T): " + (timeEnd - timeStar).Ticks);

        generator.CreateTransposeMatrix(generator.MatrixA, "A");
        generator.CreateTransposeMatrix(generator.MatrixB, "B");
        timeStar = DateTime.Now;
        CalculateMatrixMultiplication(100, "transposedMatrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
        timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения column by column: " + (timeEnd - timeStar).Ticks);

        timeStar = DateTime.Now;
        generator.CreateTransposeMatrix(generator.TransposedMatrix, "T");
        CalculateMatrixMultiplication(100, "transposedMatrixA.bin", "transposedMatrixT.bin", "resultRowColumn.bin");
        timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения column by column(T): " + (timeEnd - timeStar).Ticks);


        timeStar = DateTime.Now;
        CalculateMatrixMultiplication(100, "transposedMatrixA.bin", "matrixB.bin", "resultRowColumn.bin");
        timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения column by row: " + (timeEnd - timeStar).Ticks);

        timeStar = DateTime.Now;
        generator.CreateTransposeMatrix(generator.MatrixB, "B");
        CalculateMatrixMultiplication(100, "transposedMatrixA.bin", "transposedMatrixB.bin", "resultRowColumn.bin");
        timeEnd = DateTime.Now;
        Console.WriteLine("Время выполнения column by row(T): " + (timeEnd - timeStar).Ticks);
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
