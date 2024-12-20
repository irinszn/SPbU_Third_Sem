namespace NetworkChat;

using System.Net;
using System.Net.Sockets;

/// <summary>
/// Implementation of client for network chat.
/// </summary>
public class Client
{
    private readonly IPEndPoint endPoint;

    public Client(IPEndPoint endPoint)
    {
        this.endPoint = endPoint;
    }

    /// <summary>
    /// Starts client.
    /// </summary>
    /// <returns>Result of task.</returns>
    public async Task WorkAsync()
    {
        using var client = new TcpClient();

        Console.WriteLine($"Sending request to port {endPoint.Port}");
        await client.ConnectAsync(endPoint);
        
        using var stream = client.GetStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true };
        using var reader = new StreamReader(stream);

        await Task.WhenAny(
            ReadAsync(reader),
            WriteAsync(writer)
        );
    }

    private static async Task ReadAsync(StreamReader reader)
    {
        string? line;

        while ((line = await reader.ReadLineAsync()) != "exit")
        {
            Console.WriteLine($"Server: {line}");
        }
    }

    private static async Task WriteAsync(StreamWriter writer)
    {
        string? line;

        while ((line = Console.ReadLine()) != "exit")
        {
            await writer.WriteLineAsync(line);
        }

        await writer.WriteLineAsync("exit");
    }
}