namespace SimpleFTP;

using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Implementation of client, that supports SimpleFTP protocol.
/// </summary>
public class FTPClient : IFtpClient
{
    private readonly IPEndPoint endPoint;

    private readonly CancellationTokenSource tokenSource;

    public FTPClient(IPEndPoint endPoint)
    {
        this.endPoint = endPoint;
        this.tokenSource = new ();
    }

    /// <summary>
    /// Method that sends a request to view files in a directory on the server by entered path.
    /// </summary>
    /// <param name="request">User's request, which includes directory path.</param>
    /// <returns>List of files and directories by entered path.</returns>
    public async Task<string> ListAsync(string request)
        => await MakeRequestAsync(request);

    /// <summary>
    /// Method that sends a request to get file in a directory on the server by entered path.
    /// </summary>
    /// <param name="request">User's request, which includes file path.</param>
    /// <returns>Line with the contents of the file.</returns>
    public async Task<string> GetAsync(string request)
        => await MakeRequestAsync(request);

    /// <summary>
    /// Stops FtpClient.
    /// </summary>
    public void Stop()
    {
        tokenSource.Cancel();
        Dispose();
    }

    /// <summary>
    /// Resource release.
    /// </summary>
    public void Dispose()
    {
        tokenSource.Dispose();
    }

    /// <summary>
    /// Mathod that sends the request to server.
    /// </summary>
    /// <param name="request">User's request.</param>
    /// <returns>Response with data.</returns>
    private async Task<string> MakeRequestAsync(string request)
    {
        using var client = new TcpClient();
        Console.WriteLine($"Sending request to port {endPoint.Port}");
        await client.ConnectAsync(endPoint);

        using var stream = client.GetStream();

        using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(request);

        using var reader = new StreamReader(stream);
        var data = await reader.ReadToEndAsync(tokenSource.Token);

        return data;
    }
}