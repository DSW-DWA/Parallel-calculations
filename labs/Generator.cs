using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labs
{
    public class Generator
    {
        public int[,] MatrixA;
        public int[,] MatrixB;
        public int[,] TransposedMatrix;

        public Generator(int n)
        {
            int N = n;
            int[,] matrixA = GenerateRandomMatrix(N);
            int[,] matrixB = GenerateRandomMatrix(N);
            
            SaveMatrixToBinaryFile(matrixA, "matrixA.bin");
            SaveMatrixToBinaryFile(matrixB, "matrixB.bin");

            Console.WriteLine("Бинарные файлы matrixA.bin и matrixB.bin созданы.");

            MatrixA = matrixA;
            MatrixB = matrixB;
        }
        static int[,] GenerateRandomMatrix(int N)
        {
            Random rand = new Random();
            int[,] matrix = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    matrix[i, j] = rand.Next(1, 10);
                }
            }
            return matrix;
        }

        static void SaveMatrixToBinaryFile(int[,] matrix, string filePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                int rows = matrix.GetLength(0);
                int cols = matrix.GetLength(1);

                writer.Write(rows);
                writer.Write(cols);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        writer.Write(matrix[i, j]);
                    }
                }
            }
        }

        public void CreateTransposeMatrix(int[,] matrix, string name)
        {
            int[,] transposedMatrixA = TransposeMatrix(matrix);
            TransposedMatrix = transposedMatrixA;

            SaveMatrixToBinaryFile(transposedMatrixA, $"transposedMatrix{name}.bin");
        }
        static int[,] TransposeMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[,] transposedMatrix = new int[cols, rows];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    transposedMatrix[i, j] = matrix[j, i];
                }
            }

            return transposedMatrix;
        }

    }
}
