namespace SimpleFTP;

/// <summary>
/// Interface of client, that supports SimpleFTP protocol.
/// </summary>
public interface IFtpClient : IDisposable
{
    /// <summary>
    /// Method that sends a request to view files in a directory on the server by entered path.
    /// </summary>
    /// <param name="request">User's request, which includes directory path.</param>
    /// <returns>List of files and directories by entered path.</returns>
    public Task<string> ListAsync(string request);

    /// <summary>
    /// Method that sends a request to get file in a directory on the server by entered path.
    /// </summary>
    /// <param name="request">User's request, which includes file path.</param>
    /// <returns>Line with the contents of the file.</returns>
    public Task<string> GetAsync(string request);

    /// <summary>
    /// Stops FtpClient.
    /// </summary>
    public void Stop();
}