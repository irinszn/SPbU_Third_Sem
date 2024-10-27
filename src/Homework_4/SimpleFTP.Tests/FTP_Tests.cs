namespace SimpleFTPTests;

using SimpleFTP;
using System.Net;
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
        server = new FTPServer(endPoint.Port);
        client = new FTPClient(endPoint);

        Task.Run(() => server.Start());
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        server.Stop();
    }

    [Test]
    public async Task List_ReturnCorrectResult_WithCorrectPath()
    {
        var expected = "1 ../../SimpleFTP.Tests/TestFiles/test1.txt False\n";

        var actual = await client.List("../../SimpleFTP.Tests/TestFiles");

        Assert.That(actual, Is.EqualTo(expected));
    }
}