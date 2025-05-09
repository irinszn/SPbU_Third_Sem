namespace SimpleFTPTests;

using SimpleFTP;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;

[TestFixture]
public class Tests
{
    private FTPServer server;

    private FTPClient client;

    private IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 8888);

    [OneTimeSetUp]
    public void SetUp()
    {
        Environment.CurrentDirectory = "../../../";

        server = new FTPServer(endPoint.Port);
        client = new FTPClient(endPoint);
        Task.Run(() => server.StartAsync());
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        server.Stop();
        server.Dispose();
        client.Dispose();
    }

    [Test]
    public async Task List_ReturnExpectedResult_WithCorrectPath()
    {
        var expected1 = "2 TestFiles/List/papka True TestFiles/List/text1.txt False\n";
        var expected2 = "2 TestFiles/List/text1.txt False TestFiles/List/papka True\n";

        var expectedWin1 = "2 TestFiles/List\\papka True TestFiles/List\\text1.txt False\n";
        var expectedWin2 = "2 TestFiles/List\\text1.txt False TestFiles/List\\papka True\n";

        var actual = await client.ListAsync("1 TestFiles/List");

        Assert.That(actual, Is.EqualTo(expected1).Or.EqualTo(expected2).Or.EqualTo(expectedWin1).Or.EqualTo(expectedWin2));
    }

    [Test]
    public async Task Get_ReturnExpectedContensOfFile_WithCorrectPath()
    {
        var expectedLin = "29 some text\nsome text\nsome text";
        var expectedWin = "31 some text\r\nsome text\r\nsome text";

        var actual = await client.GetAsync("2 TestFiles/test2.txt");

        Assert.That(actual, Is.EqualTo(expectedWin).Or.EqualTo(expectedLin));
    }

    [Test]
    public async Task Get_ReturnMinusOne_WithIncorrectPath()
    {
        var expected = "-1";

        var actual = await client.GetAsync("2 TestFiles/SomeFile.txt");

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public async Task List_ReturnMinusOne_WithIncorrectPath()
    {
        var expected = "-1";

        var actual = await client.ListAsync("1 TestFiles/text1.txt");

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public async Task Get_ReturnZero_WithEmptyFile()
    {
        var expected = "0 ";

        var actual = await client.GetAsync("2 TestFiles/empty.txt");

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public async Task Get_WithManyClients_ReturnExpectedResult_Multithreaded()
    {
        const int clientsNumber = 5;
        const string request = "2 TestFiles/test1.txt";
        const int workTime = 2000;

        var results = new string[clientsNumber];
        var tasks = new Task[clientsNumber];
        var mre = new ManualResetEvent(false);

        var expected = new string[] { "4 test", "4 test", "4 test", "4 test", "4 test" };

        for (var i = 0; i < clientsNumber; ++i)
        {
            var locali = i;
            tasks[i] = Task.Run(async () =>
            {
                mre.WaitOne();

                await Task.Delay(workTime);

                var newClient = new FTPClient(endPoint);
                results[locali] = await newClient.GetAsync(request);
            });
        }

        var stopwatch = new Stopwatch();

        mre.Set();
        stopwatch.Start();

        await Task.WhenAll(tasks);

        stopwatch.Stop();

        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(workTime * clientsNumber));

        Assert.That(results, Is.EqualTo(expected));
    }
}