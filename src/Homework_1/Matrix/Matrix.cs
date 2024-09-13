namespace Matrices;

public class Matrix
{
    public Matrix(int lines, int columns)
    {
        Lines = lines;
        Columns = columns;
        Values = new int[Lines, Columns];
    }

    public Matrix()
    {
        Lines = default;
        Columns = default;
    }

    public int Columns;

    public int Lines;

    public int [,] Values;
}