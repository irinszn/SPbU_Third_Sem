using System.Threading;

var m = 3;
var n = 3;
int[,] matrix_1 = new int[m, n];
int[,] matrix_2 = new int[m, n];
int[,] result = new int[m, n];
var z = 1;

for (int i = 0; i < m; i++)
{
    for (int j = 0; j < n; j++)
    {
        matrix_1[i, j] = z;
        matrix_2[i, j] = 2*z;
        result[i, j] = 0;
        ++z;
    }
}

var threads = new Thread[m];
var chunkSize = n;

for (var i = 0; i < threads.Length; ++i)
{
    var localI = i;

    threads[i] = new Thread(() => {
        for (int j = 0; j < n; ++j)
        {
            for (int k = 0; k < n; ++k)
            {
                result[localI, j] += matrix_1[localI,k] * matrix_2[k, j];
            } 
        }
    });
}

foreach (var thread in threads)
    thread.Start();

foreach (var thread in threads)
    thread.Join();

for (int i = 0; i < m; i++)
{
    for (int j = 0; j < n; j++)
    {
        Console.Write($"{result[i, j]}  ");
    }

    Console.WriteLine();
}
