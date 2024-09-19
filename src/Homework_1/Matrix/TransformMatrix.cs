namespace Matrices;

/// <summary>
/// Class that implements reading and writing matrix to/in file.
/// </summary>
public class TransformMatrix
{
    /// <summary>
    /// Method that reads matrix from input file.
    /// </summary>
    /// <param name="filePath">Path to the file from which the matrix needs to be read.</param>
    /// <returns>Matrix object.</returns>
    public static Matrix ReadMatrixFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("No such file.");
        }

        if (new FileInfo(filePath).Length == 0)
        {
            throw new ArgumentException("File is empty.");
        }

        var matrix = new Matrix();

        var matrixLines = new List<string>();

        using var reader = new StreamReader(filePath);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line != "")
            {
                matrixLines.Add(line.ToString());
            }
        }

        matrix.Lines = matrixLines.Count;
        matrix.Columns = matrixLines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;

        matrix.Values = new int[matrix.Lines, matrix.Columns];

        for (var i = 0; i < matrix.Lines; ++i)
        {
            var elementsInLine = matrixLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            for (var j = 0; j < elementsInLine.Length; ++j)
            {
                matrix.Values[i, j] = int.Parse(elementsInLine[j]);
            }
        }

        return matrix;
    }

    /// <summary>
    /// Method that write matrix to file.
    /// </summary>
    /// <param name="matrix">Result of multiplying two matrices.</param>
    public static void WriteMatrixToFile(Matrix matrix)
    {
        using var file = new StreamWriter("result.txt");

        for (var i = 0; i < matrix.Lines; ++i)
        {
            for (var j = 0; j < matrix.Columns; ++j)
            {
                file.Write($"{matrix.Values[i, j]} ");
            }

            file.Write(Environment.NewLine);
        }
    }
}