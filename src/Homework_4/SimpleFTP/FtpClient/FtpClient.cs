namespace SimpleFTP;

using System.Net;
using System.Net.Sockets;
using System.Text;

public class FTPClient
{
    private readonly IPEndPoint endPoint;

    public FTPClient(IPEndPoint endPoint)
    {
        this.endPoint = endPoint;
    }

    public async Task<string> List(string request)
        => await MakeRequest(request);

    public async Task<string> Get(string request)
        => await MakeRequest(request);

    private async Task<string> MakeRequest(string request)
    {
        using var client = new TcpClient(endPoint);
        await client.ConnectAsync(endPoint);
        Console.WriteLine($"Sending request to port {endPoint.Port}");

        using var stream = client.GetStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(request);
        
        using var reader = new StreamReader(stream);
        var data = await reader.ReadToEndAsync();

        return data;
    }
}