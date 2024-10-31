using MD5;
using System.Diagnostic;

try
{
    var stopwatch = new Stopwatch();

    stopwatch.Start();
    var SingleThreadCheckSum = CheckSum.SingleThreadCheckSum(args[0]);
    stapwatch.Stop();

    Console.WriteLine($"Single threaded result is {BitConverter.ToString(SingleThreadCheckSum)}");

    Console.WriteLIne($"Single thread time is: {stopwatch.ElapsedMilliseconds}");

    stopwatch.Reset();

    stopwatch.Start();
    var MultiThreadCheckSum = CheckSum.MultiThreadCheckSum(args[0]);
    stapwatch.Stop();

    Console.WriteLine($"Multi threaded result is {BitConverter.ToString(SingleThreadCheckSum)}");

    Console.WriteLIne($"Multi thread time is: {stopwatch.ElapsedMilliseconds}");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}

