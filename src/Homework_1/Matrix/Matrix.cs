namespace Matrices;

/// <summary>
/// Class that implements structures of matrix.
/// </summary>
public class Matrix
{
     /// <summary>
    /// Number of columns in matrix.
    /// </summary>
    public int Columns;

    /// <summary>
    /// Number of lines in matrix.
    /// </summary>
    public int Lines;

    /// <summary>
    /// Matrix in array.
    /// </summary>
    public int[,] Values;

    /// <summary>
    /// Matrix structure as two-dimensional array with number of lines and columns.
    /// </summary>
    /// <param name="lines">Number of lines in matrix.</param>
    /// <param name="columns">Number of columns in matrix.</param>
    public Matrix(int lines, int columns)
    {
        Lines = lines;
        Columns = columns;
        Values = new int[Lines, Columns];
    }
}