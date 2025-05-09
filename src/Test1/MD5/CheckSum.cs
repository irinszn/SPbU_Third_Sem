namespace CheckSumMD5;

using System.Security.Cryptography;
using static System.Text.Encoding;

/// <summary>
/// Class that calculates the checksum of a filesystem directory.
/// </summary>
public class CheckSum
{
    /// <summary>
    /// Method that calculates the checksum single threaded.
    /// </summary>
    /// <param name="path">File or directory path.</param>
    /// <returns>Array of bytes.</returns>
    public static byte[] SingleThreadCheckSum(string path)
    {
        if (File.Exists(path))
        {
            return GetFileHash(path);
        }

        if (Directory.Exists(path))
        {
            return GetDirectoryHash(path);
        }

        throw new FileNotFoundException("There is no such file or directory.");
    }

    /// <summary>
    /// Method that calculates the checksum multi-threaded.
    /// </summary>
    /// <param name="path">File or directory path.</param>
    /// <returns>Array of bytes.</returns>
    public static async Task<byte[]> MultiThreadCheckSum(string path)
    {
        if (File.Exists(path))
        {
            return GetFileHash(path);
        }

        if (Directory.Exists(path))
        {
            return await GetDirectoryHashAsync(path);
        }

        throw new FileNotFoundException("There is no such file or directory.");
    }

    /// <summary>
    /// Method that gets hash of the file using MD5.
    /// </summary>
    /// <param name="path">File path.</param>
    /// <returns>Byte array with hash.</returns>
    private static byte[] GetFileHash(string path)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(path);
        var hash = md5.ComputeHash(stream);

        return hash;
    }

    /// <summary>
    /// Method that gets hash of the directory using MD5.
    /// </summary>
    /// <param name="path">Directory path.</param>
    /// <returns>Byte array with directory hash.</returns>
    private static byte[] GetDirectoryHash(string path)
    {
        using var md5 = MD5.Create();

        var subDirectories = Directory.GetDirectories(path);
        var filesInDirectory = Directory.GetFiles(path);

        Array.Sort(subDirectories);
        Array.Sort(filesInDirectory);

        var resultHashes = new List<byte>(UTF8.GetBytes(Path.GetFileName(path) ?? string.Empty));

        foreach (var file in filesInDirectory)
        {
            resultHashes.AddRange(GetFileHash(file));
        }

        foreach (var elem in subDirectories)
        {
            resultHashes.AddRange(GetDirectoryHash(elem));
        }

        return resultHashes.ToArray();
    }

    /// <summary>
    /// Method that gets hash of the directory async using MD5.
    /// </summary>
    /// <param name="path">Directory path.</param>
    /// <returns>Task.</returns>
    private static async Task<byte[]> GetDirectoryHashAsync(string path)
    {
        using var md5 = MD5.Create();

        var subDirectories = Directory.GetDirectories(path);
        var filesInDirectory = Directory.GetFiles(path);

        Array.Sort(subDirectories);
        Array.Sort(filesInDirectory);

        var directoriesHashes = new Task<byte[]> [subDirectories.Length];
        var filesHashes = new Task<byte[]> [filesInDirectory.Length];

        for (var i = 0; i < subDirectories.Length; ++i)
        {
            directoriesHashes[i] = GetDirectoryHashAsync(subDirectories[i]);
        }

        for (var i = 0; i < filesInDirectory.Length; ++i)
        {
            filesHashes[i] = GetDirectoryHashAsync(filesInDirectory[i]);
        }

        await Task.WhenAll(directoriesHashes);
        await Task.WhenAll(filesHashes);

        var resultHashes = new List<byte>(UTF8.GetBytes(Path.GetFileName(path) ?? string.Empty));

        foreach (var fileHash in filesHashes)
        {
            resultHashes.AddRange(fileHash.Result);
        }

        foreach (var directoryHash in directoriesHashes)
        {
            resultHashes.AddRange(directoryHash.Result);
        }

        return resultHashes.ToArray();
    }
}
