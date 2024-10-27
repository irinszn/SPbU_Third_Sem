namespace SimpleFTP;

using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Implemetation of the server which supports SimpleFTP protocol.
/// </summary>
public class FTPServer
{
    private readonly CancellationTokenSource tokenSource;
    private readonly TcpListener listener;
    private readonly int port;

    public FTPServer(int port)
    {
        this.port = port;
        listener = new TcpListener(IPAddress.Any, port);
        tokenSource = new();
    }

    /// <summary>
    /// Method that accepts clients requests.
    /// </summary>
    /// <returns>Result of asynchronous task execution.</returns>
    public async Task Start()
    {
        var tasksList = new List<Task>();

        listener.Start();
        Console.WriteLine($"Server started on port {port}");

        while (!tokenSource.IsCancellationRequested)
        {
            try
            {
                var client = await listener.AcceptTcpClientAsync(tokenSource.Token);
                Console.WriteLine("Connection is established.");

                tasksList.Add(HandleRequests(client));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in processing the request: {ex.Message}.");
            }
        }

        await Task.WhenAll(tasksList);
    }

    /// <summary>
    /// Method that stops server.
    /// </summary>
    public void Stop()
    {
        tokenSource.Cancel();
        listener.Stop();
    }

    /// <summary>
    /// Method that processes requests.
    /// </summary>
    private async Task HandleRequests(TcpClient client)
    {
        using var stream = client.GetStream();
        using var reader = new StreamReader(stream);
        using var writer = new StreamWriter(stream) { AutoFlush = true};

        var data = await reader.ReadLineAsync();

        if (data != null)
        {
            if (data[..2] == "1 ")
            {
                Console.WriteLine("I'm here");
                await List(data[2..], writer);
            }

            if (data[..2] == "2 ")
            {
                await Get(data[2..], writer);
            }
        }
    }

    /// <summary>
    /// Method that sends content of file to the client.
    /// </summary>
    /// <param name="path">File path in the request.</param>
    /// <returns>Result of asynchronous task execution.</returns>
    private static async Task Get(string path, StreamWriter writer)
    {
        if (!File.Exists(path))
        {
            await writer.WriteAsync("-1");
            return;
        }

        var content = await File.ReadAllBytesAsync(path);
        await writer.WriteAsync($"{content.Length} {Encoding.UTF8.GetString(content)}");
    }

    /// <summary>
    /// Method that sends list of files and directories to the client.
    /// </summary>
    /// <param name="path">Directory path in the request.</param>
    /// <returns>Result of asynchronous task execution.</returns>
    private static async Task List(string path, StreamWriter writer)
    {
        if (!Directory.Exists(path))
        {
            await writer.WriteAsync("-1");
            return;
        }

        var fileAndDirectories = Directory.GetFileSystemEntries(path);
        var size = fileAndDirectories.Length;

        await writer.WriteAsync($"{size}");

        foreach (var item in fileAndDirectories)
        {
            await writer.WriteAsync($" {item} {Directory.Exists(item)}");
        }

        await writer.WriteAsync("\n");
    }
}