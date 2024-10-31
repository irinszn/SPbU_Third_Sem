using CheckSumMD5;
using System.Diagnostics;

try
{
    var stopwatch = new Stopwatch();

    stopwatch.Start();
    var SingleThreadCheckSum = CheckSum.SingleThreadCheckSum(args[0]);
    stopwatch.Stop();

    Console.WriteLine($"Single threaded result is {BitConverter.ToString(SingleThreadCheckSum)}");

    Console.WriteLine($"Single thread time is: {stopwatch.ElapsedMilliseconds}");

    stopwatch.Reset();

    stopwatch.Start();
    var MultiThreadCheckSum = CheckSum.MultiThreadCheckSum(args[0]);
    stopwatch.Stop();

    Console.WriteLine($"Multi threaded result is {BitConverter.ToString(SingleThreadCheckSum)}");

    Console.WriteLine($"Multi thread time is: {stopwatch.ElapsedMilliseconds}");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}

