namespace SimpleFTP;

/// <summary>
/// Interface of the server which supports SimpleFTP protocol.
/// </summary>
public interface IFtpServer : IDisposable
{
    /// <summary>
    /// Method that accepts clients requests.
    /// </summary>
    /// <returns>Result of asynchronous task execution.</returns>
    public Task StartAsync();

    /// <summary>
    /// Method that stops server.
    /// </summary>
    public void Stop();
}