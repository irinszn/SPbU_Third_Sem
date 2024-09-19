namespace Matrices;

using System.Diagnostics;

/// <summary>
/// Class that implements matrix multiplication analysis.
/// </summary>
public class MultiplicationAnalysis
{
    private static readonly Stopwatch Stopwatch = new ();

    public static void Analyze()
    {
        var dimensions = new (int, int)[] { (300, 300), (500, 500), (1000, 1000) };
        var parallelResults = new List<(double, double)>();
        var usualResults = new List<(double, double)>();

        foreach (var dim in dimensions)
        {
            parallelResults.Add(AnalyzeMultiplication(dim, dim, true));
        }

        foreach (var dim in dimensions)
        {
            usualResults.Add(AnalyzeMultiplication(dim, dim, false));
        }

        Console.WriteLine("Values ​​of expectation and standard deviation for matrix multiplication (in ms):");
        Console.WriteLine("");
        Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20}", "Dimensions", "300×300", "500×500", "1000×1000");
        Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20}", "Parallel", $"{parallelResults[0].Item1}, {parallelResults[0].Item2} ", $"{parallelResults[1].Item1}, {parallelResults[1].Item2} ", $"{parallelResults[2].Item1}, {parallelResults[2].Item2} ");
        Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20}", "Not parallel", $"{usualResults[0].Item1}, {usualResults[0].Item2} ", $"{usualResults[1].Item1}, {usualResults[1].Item2} ", $"{usualResults[2].Item1}, {usualResults[2].Item2} ");
    }

    private static (double, double) AnalyzeMultiplication((int, int) firstMatrixSize, (int, int) secondMatrixSize, bool isParallel)
    {
        var matrix_1 = Generate.GenerateMatrix(firstMatrixSize.Item1, firstMatrixSize.Item2);
        var matrix_2 = Generate.GenerateMatrix(secondMatrixSize.Item1, secondMatrixSize.Item2);

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

        double standardDeviation = Math.Round(Math.Sqrt(variance), 4);

        return (mean, standardDeviation);
    }
}