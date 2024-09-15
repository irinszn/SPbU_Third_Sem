using Matrices;

Generate.GenerateMatrix(3, 4, "matrix_1.txt");
Generate.GenerateMatrix(4, 5, "matrix_2.txt");

var matrix_1 = TransformMatrix.ReadMatrixFromFile("matrix_1.txt");
var matrix_2 = TransformMatrix.ReadMatrixFromFile("matrix_2.txt");


var result = MatrixMultiplication.ParallelMultiplication(matrix_1, matrix_2);
//var result = MatrixMultiplication.Multiplication(matrix_1, matrix_2);

TransformMatrix.WriteMatrixToFile(result);

