namespace Matrices;

/// <summary>
/// Class that implement generation of matrices of specified length and width.
/// </summary>
public class Generate
{
    /// <summary>
    /// Method to generate matrix.
    /// </summary>
    /// <param name="lines">Number of lines in matrix.</param>
    /// <param name="columns">Number of columns in matrix.</param>
    /// <param name="filePath">Name of the file into which the matrix should be written.</param>
    public static void GenerateMatrix(int lines, int columns, string filePath)
    {
        Random rnd = new Random();

        using var file = new StreamWriter(filePath);

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                file.Write($"{rnd.Next(-100, 100)} ");
            }

            file.Write(Environment.NewLine);
        }
    }
}