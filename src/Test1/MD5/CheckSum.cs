namespace CheckSumMD5;

using System.Security.Cryptography;
using System.IO;
using static System.Text.Encoding;

public class CheckSum
{
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

    private static byte[] GetFileHash(string path)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(path);
        var hash = md5.ComputeHash(stream);

        return hash;
    }

    private static byte[] GetDirectoryHash(string path)
    {
        using var md5 = MD5.Create();

        var subDirectories = Directory.GetDirectories(path);
        var filesInDirectory = Directory.GetFiles(path);

        Array.Sort(subDirectories);
        Array.Sort(filesInDirectory);

        var resultHashes = new List<byte>();

        resultHashes.Add(UTF8.GetBytes(Path.GetFileName(path) ?? string.Empty));

        foreach (var file in filesInDirectory)
        {
            resultHashes.Add(GetFileHash(file));
        }

        foreach (var elem in subDirectories)
        {
            resultHashes.Add(GetDirectoryHash(elem));
        }

        return resultHashes.ToArray();
    }

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

        var resultHashes = new List<byte>();
        resultHashes.Add(UTF8.GetBytes(Path.GetFileName(path) ?? string.Empty));

        foreach (var fileHash in filesHashes)
        {
            resultHashes.Add(fileHash.Result);
        }

        foreach (var directoryHash in directoriesHashes)
        {
            resultHashes.Add(directoryHash.Result);
        }

        return resultHashes.ToArray();
    }
}
