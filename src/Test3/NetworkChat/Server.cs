namespace NetworkChat;

using System.Net;
using System.Net.Sockets;

/// <summary>
/// Implementation of server for network chat.
/// </summary>
public class Server
{
    private readonly CancellationTokenSource tokenSource;
    private readonly TcpListener listener;
    private readonly int port;

    public Server(int port)
    {
        this.port = port;
        listener = new TcpListener(IPAddress.Any, port);
        tokenSource = new ();
    }

    /// <summary>
    /// Starts server.
    /// </summary>
    /// <returns>Result of task.</returns>
    public async Task StartAsync()
    {
        listener.Start();
        Console.WriteLine($"Server started on port {port}");

        while (!tokenSource.IsCancellationRequested)
        {
            try
            {
                var client = await listener.AcceptTcpClientAsync(tokenSource.Token);
                Console.WriteLine("Connection is established.");

                using var stream = client.GetStream();
                using var reader = new StreamReader(stream);
                using var writer = new StreamWriter(stream) { AutoFlush = true };

                await Task.WhenAny(
                    ReadAsync(reader),
                    WriteAsync(writer));
                
                Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in processing: {ex.Message}.");
            }
        }
    }

    private static async Task ReadAsync(StreamReader reader)
    {
        string? line;

        while ((line = await reader.ReadLineAsync()) != "exit")
        {
            Console.WriteLine($"Client: {line}");
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

    private void Stop()
    {
        tokenSource.Cancel();
        listener.Stop();
    }
}