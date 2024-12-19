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
        line = await reader.ReadLineAsync();

        while (line != "exit")
        {
            Console.WriteLine($"Server: {line}");
            line = await reader.ReadLineAsync();
        }
    }

    private static async Task WriteAsync(StreamWriter writer)
    {
        string? line;
        line = Console.ReadLine();

        while (line != "exit")
        {
            await writer.WriteLineAsync($"Client: {line}");
            line = Console.ReadLine();

        }
        await writer.WriteLineAsync("exit");
    }
}