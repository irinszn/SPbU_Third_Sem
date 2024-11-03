namespace Matrices;

using System.Diagnostics;

/// <summary>
/// Class that implements matrix multiplication analysis.
/// </summary>
public class MultiplicationAnalysis
{
    public static void PrintAnalysis()
    {
        var parallelResults = new List<(int, int)> { (300, 300), (500, 500), (1000, 1000) };
        var usualResults = new List<(int, int)> { (300, 300), (500, 500), (1000, 1000) };

        parallelResults.Select(x => AnalyzeMultiplication(x, x, true));

        usualResults.Select(x => AnalyzeMultiplication(x, x, false));

        Console.WriteLine("Values of expectation and standard deviation for matrix multiplication (in ms):");
        Console.WriteLine("");
        Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20}", "Dimensions", "300×300", "500×500", "1000×1000");
        Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20}", "Parallel", $"{parallelResults[0].Item1}, {parallelResults[0].Item2} ", $"{parallelResults[1].Item1}, {parallelResults[1].Item2} ", $"{parallelResults[2].Item1}, {parallelResults[2].Item2} ");
        Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20}", "Not parallel", $"{usualResults[0].Item1}, {usualResults[0].Item2} ", $"{usualResults[1].Item1}, {usualResults[1].Item2} ", $"{usualResults[2].Item1}, {usualResults[2].Item2} ");
    }

    private static (double, double) AnalyzeMultiplication((int, int) firstMatrixSize, (int, int) secondMatrixSize, bool isParallel)
    {
        var matrix_1 = MatrixGenerateHelper.GenerateMatrix(firstMatrixSize.Item1, firstMatrixSize.Item2);
        var matrix_2 = MatrixGenerateHelper.GenerateMatrix(secondMatrixSize.Item1, secondMatrixSize.Item2);

        var results = new List<double>();
        const int launchNumber = 10;

        var stopwatch = new Stopwatch();

        for (var i = 0; i < launchNumber; ++i)
        {
            if (isParallel)
            {
                stopwatch.Start();
                MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2);
                stopwatch.Stop();
            }
            else
            {
                stopwatch.Start();
                MatrixMultiplication.Multiplication(matrix_1, matrix_2);
                stopwatch.Stop();
            }

            results.Add(stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();
        }

        double mean = results.Average();

        double variance = results.Sum(r => Math.Pow(r - mean, 2)) / launchNumber;

        double standardDeviation = Math.Round(Math.Sqrt(variance), 4);

        return (mean, standardDeviation);
    }
}