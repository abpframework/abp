using System;
using System.IO;

namespace Volo.Abp.Cli.Utils;

/// <summary>
/// A folder in the temp directory, deleted when disposed.
/// </summary>
public sealed class TemporaryDirectory : IDisposable
{
    public string Path { get; }

    public TemporaryDirectory()
    {
        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path);
    }

    /// <summary>
    /// Creates a file (and its folders) below the directory and returns the full path of the folder it is in.
    /// </summary>
    public string CreateFile(string relativePath, string content = "")
    {
        var filePath = System.IO.Path.Combine(Path, relativePath);
        var directory = System.IO.Path.GetDirectoryName(filePath)!;

        Directory.CreateDirectory(directory);
        File.WriteAllText(filePath, content);

        return directory;
    }

    public void Dispose()
    {
        Directory.Delete(Path, true);
    }
}
