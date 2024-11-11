using MyNUnit;
 
public class SomeClass
{
    private int _value;

    [BeforeClass]
    public static void BeforeClass()
    {
        Console.WriteLine("SetUp before tests");
    }

    [Before]
    public void Before()
    {
        Console.WriteLine("Method before test");
        _value = 5;
    }

    [After]
    public void After()
    {
        Console.WriteLine("Method after test");
    }

    [AfterClass]
    public static void AfterClass()
    {
        Console.WriteLine("TearDown after tests");
    }

    [Test]
    public void BeforeMethodWasCalled()
    {
        if (_value != 5)
        {
            Console.WriteLine(_value);
            throw new Exception("Before method wasn't called");
        }
    }

    [Test]
    public void AssertWithTrueExpressionPassed()
    {
        Assert.That(_value == 5);
    }

    [Test(typeof(InvalidOperationException))]
    public void ExpectedExceptionPassed()
    {
        throw new InvalidOperationException();
    }

    [Test(null, "Ignored because of some reason")]
    public void IgnoredTestPassed()
    {
        throw new Exception("This test should be ignored");
    }

    [Test]
    public void TestFalledWithException()
    {
        throw new Exception("This test should fall");
    }

    [Test]
    public void TestFallAfterAssert()
    {
        Assert.That(5 == 6);
    }
}