namespace Matrices;

/// <summary>
/// Class that implement generation of matrices of specified length and width.
/// </summary>
public static class MatrixGenerateHelper
{
    /// <summary>
    /// Method that generates matrix.
    /// </summary>
    /// <param name="lines">Number of lines in matrix.</param>
    /// <param name="columns">Number of columns in matrix.</param>
    /// <param name="filePath">Name of the file into which the matrix should be written.</param>
    public static void GenerateMatrixInFile(int lines, int columns, string filePath)
    {
        using var file = new StreamWriter(filePath);

        var matrix = GenerateMatrix(lines, columns);

        for (int i = 0; i < lines; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                file.Write($"{matrix.Values[i, j]} ");
            }

            file.Write(Environment.NewLine);
        }
    }

    /// <summary>
    /// Method that generates matrix.
    /// </summary>
    /// <param name="lines">Number of lines in matrix.</param>
    /// <param name="columns">Number of columns in matrix.</param>
    /// <returns>Matrix.</returns>
    public static Matrix GenerateMatrix(int lines, int columns)
    {
        Random rnd = new Random();

        var matrix = new Matrix(lines, columns);

        for (int i = 0; i < lines; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                matrix.Values[i, j] = rnd.Next(-100, 100);
            }
        }

        return matrix;
    }
}