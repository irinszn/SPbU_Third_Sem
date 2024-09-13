namespace Matrices;

public class Generate
{
    public static void GenerateMatrix(int lines, int columns, string filePath)
    {
        Random rnd = new Random();

        string matrix = "";

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                matrix += rnd.Next(-100, 100).ToString();
                matrix += " ";
            }

            matrix += "\n";
        }

        File.WriteAllText(filePath, matrix);
    }
}