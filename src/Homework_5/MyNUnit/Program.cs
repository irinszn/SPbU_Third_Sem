using MyNUnit;

if (args.Length != 1)
{
    Console.WriteLine("Incorect input. Enter path.");
}

var path = args[0];

var runner = new TestRunner();
runner.RunTest(path);