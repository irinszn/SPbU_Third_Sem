namespace Tests;

using Matrices;

public class MatrixTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        Environment.CurrentDirectory = "../../../";
    }

    [TestCase("Files/MatrixWithManySpaces.txt")]
    public void ReadMatrixFromFile_ReturnExpectedValues(string filePath)
    {
        int[,] expected = { {1, 2, 3}, {4, 5, 6}, {7, 8, 9} };

        var matrix = MatrixTransformHelper.ReadMatrixFromFile(filePath);

        Assert.That(matrix.Values, Is.EqualTo(expected));
    }

    [TestCase("result.txt", "Files/Write_result.txt")]
    public void WriteMatrixToFile_WriteExpectedValues(string result, string expectedResult)
    {
        var matrix = new Matrix(3, 3);
        int[,] values = { {1, 2, 3}, {4, 5, 6}, {7, 8, 9} };

        matrix.Values = values;
        MatrixTransformHelper.WriteMatrixToFile(matrix);

        Assert.That(MatrixTransformHelper.ReadMatrixFromFile(result).Values, Is.EqualTo(MatrixTransformHelper.ReadMatrixFromFile(expectedResult).Values));
    }

    [TestCase("Files/matrix_1.txt", "Files/matrix_2.txt", "Files/result_12.txt")]
    [TestCase("Files/matrix_11.txt", "Files/matrix_22.txt", "Files/result_1122.txt")]

    public void Multiplication_ReturnExpectedResult(string matrix_one, string matrix_two, string expectedResult)
    {
        var matrix_1 = MatrixTransformHelper.ReadMatrixFromFile(matrix_one);
        var matrix_2 = MatrixTransformHelper.ReadMatrixFromFile(matrix_two);
        var expected = MatrixTransformHelper.ReadMatrixFromFile(expectedResult);

        var result = MatrixMultiplication.Multiplication(matrix_1, matrix_2);

        Assert.That(result.Values, Is.EqualTo(expected.Values));
    }

    [TestCase("Files/matrix_1.txt", "Files/matrix_2.txt", "Files/result_12.txt")]
    [TestCase("Files/matrix_11.txt", "Files/matrix_22.txt", "Files/result_1122.txt")]
    public void ParallelMultiplication_ReturnExpectedResult(string matrix_one, string matrix_two, string expectedResult)
    {
        var matrix_1 = MatrixTransformHelper.ReadMatrixFromFile(matrix_one);
        var matrix_2 = MatrixTransformHelper.ReadMatrixFromFile(matrix_two);
        var expected = MatrixTransformHelper.ReadMatrixFromFile(expectedResult);

        var result = MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2);

        Assert.That(result.Values, Is.EqualTo(expected.Values));
    }

    [Test]
    public void ParallelMultiplication_ThrowsArgumentException_WhenMatricesNotConsistent()
    {
        var matrix_1  = new Matrix(3, 4);
        var matrix_2  = new Matrix(5, 6);

        Assert.Throws<ArgumentException>(() => MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2));
    }

    [Test]
    public void ReadMatrixFromFile_ThrowsFileNotFoundException_WithWrongFilePath()
    {
        Assert.Throws<FileNotFoundException>(() => MatrixTransformHelper.ReadMatrixFromFile("Files/Flower.txt"));
    }

    [Test]
    public void ReadMatrixFromFile_ThrowsArgumentException_WithEmptyFile()
    {
        Assert.Throws<ArgumentException>(() => MatrixTransformHelper.ReadMatrixFromFile("Files/EmptyFile.txt"));
    }
}