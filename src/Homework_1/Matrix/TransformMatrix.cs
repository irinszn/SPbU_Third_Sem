namespace Matrices;

public class TransformMatrix
{

    public static Matrix ReadMatrixFromFile(string filePath)
    {
        var matrix = new Matrix();

        var matrixLines = File.ReadAllText(filePath).Split("\n", StringSplitOptions.RemoveEmptyEntries);

        matrix.Lines = matrixLines.Length;
        matrix.Columns = matrixLines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;

        matrix.Values = new int[matrix.Lines, matrix.Columns];

        for (var i = 0; i < matrix.Lines; ++i)
        {
            var elementsInLine = matrixLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            for (var j = 0; j < elementsInLine.Length; ++j)
            {
                matrix.Values[i,j] = int.Parse(elementsInLine[j]);
            }
        }
        
        return matrix;
    }

    public static void WriteMatrixToFile(Matrix matrix)
    {
        var result = "";

        for (var i = 0; i < matrix.Lines; ++i)
        {
            for (var j = 0; j < matrix.Columns; ++j)
            {
                result += matrix.Values[i,j] + " ";
            }

            result += "\n";
        }

        File.WriteAllText("result", result);
    }
}