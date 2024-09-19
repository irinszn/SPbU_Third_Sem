namespace Matrices;

using System.Diagnostics;

public class MultiplicationAnalysis
{
    private static readonly Stopwatch Stopwatch = new ();

    public static void Analyze()
    {
        var dimensions = new (int, int)[] { (500, 500), (1000, 1000), (1500, 1500) };
        var parallelResults = new List<(double, double)>();
        var usualResult = new List<(double, double)>();

        foreach (var dim in dimensions)
        {
            parallelResults.Add(AnalyzeMultiplication(dim, dim, true));
        }

        foreach (var dim in dimensions)
        {
            parallelResults.Add(AnalyzeMultiplication(dim, dim, false));
        }
    }

    private static (double, double) AnalyzeMultiplication((int, int) firstMatrixSize, (int, int) secondMatrixSize, bool isParallel)
    {
        Stopwatch.Reset();

        Generate.GenerateMatrix(firstMatrixSize.Item1, firstMatrixSize.Item2, "matrix_1.txt");
        Generate.GenerateMatrix(secondMatrixSize.Item1, secondMatrixSize.Item2, "matrix_2.txt");

        var matrix_1 = TransformMatrix.ReadMatrixFromFile("matrix_1.txt");
        var matrix_2 = TransformMatrix.ReadMatrixFromFile("matrix_2.txt");

        var results = new List<double>();
        const int startsNumber = 10;

        for (var i = 0; i < startsNumber; ++i)
        {
            if (isParallel)
            {
                Stopwatch.Start();
                MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2);
                Stopwatch.Stop();
            }
            else
            {
                Stopwatch.Start();
                MatrixMultiplication.Multiplication(matrix_1, matrix_2);
                Stopwatch.Stop();
            }

            results.Add(Stopwatch.ElapsedMilliseconds);
            Stopwatch.Reset();
        }

        double mean = results.Average();

        double variance = results.Sum(r => Math.Pow(r - mean, 2)) / startsNumber;

        double standardDeviation = Math.Sqrt(variance);

        Console.WriteLine($"Expected execution time (ms): {mean}");
        Console.WriteLine($"Execution Time Standard Deviation (ms): {standardDeviation}");

        return (mean, standardDeviation);
    }
}