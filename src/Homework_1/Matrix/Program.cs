using Matrices;

if (args.Length == 0 || args[0] == "-help")
{
    Console.WriteLine("""
                        This program multiplies matrices.
    
                        1. To generate two matrices for multiplication:

                        dotnet run -generate [number of lines of 1st matrix] [number of columns of 1st matrix] [... 2nd matrix] [... 2nd matrix]

                        2. To multiply your own matrices:

                        dotnet run [first matrix file path] [second matrix file path]

                        3. To get analysis of multiplication:

                        dotnet run -analyze

                        """);

    return 0;
}

if (args.Length == 5 & args[0] == "-generate")
{
    try
    {
        MatrixGenerateHelper.GenerateMatrixInFile(int.Parse(args[1]), int.Parse(args[2]), "matrix_1.txt");
        MatrixGenerateHelper.GenerateMatrixInFile(int.Parse(args[3]), int.Parse(args[4]), "matrix_2.txt");

        var matrix_1 = MatrixTransformHelper.ReadMatrixFromFile("matrix_1.txt");
        var matrix_2 = MatrixTransformHelper.ReadMatrixFromFile("matrix_2.txt");

        var result = MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2);

        MatrixTransformHelper.WriteMatrixToFile(result);

        Console.WriteLine("Successful. Result of multiplication in result.txt file.");
    }
    catch (ArgumentException e)
    {
        Console.WriteLine(e.Message);
    }
}
else if (args.Length == 2)
{
    try
    {
        var matrix_1 = MatrixTransformHelper.ReadMatrixFromFile("matrix_1.txt");
        var matrix_2 = MatrixTransformHelper.ReadMatrixFromFile("matrix_2.txt");

        var result = MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2);

        MatrixTransformHelper.WriteMatrixToFile(result);

        Console.WriteLine("Successful. Result of multiplication in result.txt file.");
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine("There is no such file.");
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Matrices are not consistent or file is empty.");
    }
}
else if (args[0] == "-analyze")
{
    MultiplicationAnalysis.PrintAnalysis();
}
else
{
    Console.WriteLine("Something went wrong. Enter dotnet -help to see more.");
}

return 0;