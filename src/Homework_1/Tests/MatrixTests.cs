namespace Tests;

using Matrices;

public class MatrixTests
{
    [TestCase("../../../Files/MatrixWithManySpaces.txt")]
    public void ReadMatrixFromFile_WorksCorrectly(string filePath)
    {
        int[,] expected = { {1, 2, 3}, {4, 5, 6}, {7, 8, 9} };

        var matrix = TransformMatrix.ReadMatrixFromFile(filePath);

        Assert.AreEqual(expected, matrix.Values);
    }

    [TestCase("result.txt", "../../../Files/Write_result.txt")]
    public void WriteMatrixToFile_WorksCorrectly(string result, string expectedResult)
    {
        var matrix = new Matrix(3, 3);
        int[,] values = { {1, 2, 3}, {4, 5, 6}, {7, 8, 9} };

        matrix.Values = values;
        TransformMatrix.WriteMatrixToFile(matrix);

        Assert.AreEqual(TransformMatrix.ReadMatrixFromFile(result).Values, TransformMatrix.ReadMatrixFromFile(expectedResult).Values);
    }

    [TestCase("../../../Files/matrix_1.txt", "../../../Files/matrix_2.txt", "../../../Files/result_12.txt")]
    [TestCase("../../../Files/matrix_11.txt", "../../../Files/matrix_22.txt", "../../../Files/result_1122.txt")]

    public void Multiplication_WorksCorrectly(string matrix_one, string matrix_two, string expectedResult)
    {
        var matrix_1 = TransformMatrix.ReadMatrixFromFile(matrix_one);
        var matrix_2 = TransformMatrix.ReadMatrixFromFile(matrix_two);
        var expected = TransformMatrix.ReadMatrixFromFile(expectedResult);

        var result = MatrixMultiplication.Multiplication(matrix_1, matrix_2);
        
        Assert.AreEqual(result.Values, expected.Values);
    }

    [TestCase("../../../Files/matrix_1.txt", "../../../Files/matrix_2.txt", "../../../Files/result_12.txt")]
    [TestCase("../../../Files/matrix_11.txt", "../../../Files/matrix_22.txt", "../../../Files/result_1122.txt")]
    public void ParallelMultiplication_WorksCorrectly(string matrix_one, string matrix_two, string expectedResult)
    {
        var matrix_1 = TransformMatrix.ReadMatrixFromFile(matrix_one);
        var matrix_2 = TransformMatrix.ReadMatrixFromFile(matrix_two);
        var expected = TransformMatrix.ReadMatrixFromFile(expectedResult);

        var result = MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2);
        
        Assert.AreEqual(result.Values, expected.Values);
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
        Assert.Throws<FileNotFoundException>(() => TransformMatrix.ReadMatrixFromFile("../../../Files/Flower.txt"));
    }

    [Test]
    public void ReadMatrixFromFile_ThrowsArgumentException_WithEmptyFile()
    {
        Assert.Throws<ArgumentException>(() => TransformMatrix.ReadMatrixFromFile("../../../Files/EmptyFile.txt"));
    }
}