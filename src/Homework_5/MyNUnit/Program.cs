using MyNUnit;

if (args.Length != 1)
{
    Console.WriteLine("Incorect input. Enter path.");
    return 1;
}

var path = args[0];

var runner = new TestRunner();
runner.RunTest(path);

return 0;