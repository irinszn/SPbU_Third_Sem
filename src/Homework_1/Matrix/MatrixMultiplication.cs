namespace Matrices;

using System.Threading;

/// <summary>
/// Class that implements multiplying two matrices.
/// </summary>
public class MatrixMultiplication
{
    /// <summary>
    /// Method that multiplies two matrices single-threaded.
    /// </summary>
    /// <param name="matrix_1">First matrix.</param>
    /// <param name="matrix_2">Second matrix.</param>
    /// <returns>Resulting multiplication matrix.</returns>
    public static Matrix Multiplication(Matrix matrix_1, Matrix matrix_2)
    {
        if (matrix_1.Columns != matrix_2.Lines)
        {
            throw new ArgumentException("Matrices are not consistent.");
        }

        var result = new Matrix(matrix_1.Lines, matrix_2.Columns);

        for (var i = 0; i < matrix_1.Lines; ++i)
        {
            for (var j = 0; j < matrix_2.Columns; ++j)
            {
                for (var k = 0; k < matrix_1.Columns; ++k)
                {
                    result.Values[i, j] += matrix_1.Values[i, k] * matrix_2.Values[k, j];
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Method that multiplies two matrices multithreaded.
    /// </summary>
    /// <param name="matrix_1">First matrix.</param>
    /// <param name="matrix_2">Second matrix.</param>
    /// <returns>Resulting multiplication matrix.</returns>
    public static Matrix ParallelMultiplication(Matrix matrix_1, Matrix matrix_2)
    {
        if (matrix_1.Columns != matrix_2.Lines)
        {
            throw new ArgumentException("Matrices are not consistent.");
        }

        var result = new Matrix(matrix_1.Lines, matrix_2.Columns);

        var threads = new Thread[Environment.ProcessorCount];
        var chunkSize = (matrix_1.Lines / Environment.ProcessorCount) + 1;

        for (var i = 0; i < threads.Length; ++i)
        {
            var localI = i;

            threads[i] = new Thread(() =>
            {
                for (var i = localI * chunkSize; i < (localI + 1) * chunkSize & i < matrix_1.Lines; ++i)
                {
                    for (var j = 0; j < matrix_2.Columns; ++j)
                    {
                        for (var k = 0; k < matrix_1.Columns; ++k)
                        {
                            result.Values[i, j] += matrix_1.Values[i, k] * matrix_2.Values[k, j];
                        }
                    }
                }
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return result;
    }
}