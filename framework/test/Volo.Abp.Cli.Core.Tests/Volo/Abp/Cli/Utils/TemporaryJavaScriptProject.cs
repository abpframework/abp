using System;
using System.IO;

namespace Volo.Abp.Cli.Utils;

/// <summary>
/// A JavaScript project folder in the temp directory, deleted when disposed.
/// </summary>
public sealed class TemporaryJavaScriptProject : IDisposable
{
    public string Directory { get; }

    public string PackageJsonFilePath => Path.Combine(Directory, "package.json");

    public TemporaryJavaScriptProject(string packageJsonContent, params string[] emptyFiles)
    {
        Directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        System.IO.Directory.CreateDirectory(Directory);

        File.WriteAllText(PackageJsonFilePath, packageJsonContent);

        foreach (var emptyFile in emptyFiles)
        {
            File.WriteAllText(Path.Combine(Directory, emptyFile), string.Empty);
        }
    }

    public void WriteFile(string relativePath, string content)
    {
        var path = Path.Combine(Directory, relativePath);
        System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, content);
    }

    public string ReadFile(string relativePath)
    {
        return File.ReadAllText(Path.Combine(Directory, relativePath));
    }

    public void Dispose()
    {
        System.IO.Directory.Delete(Directory, true);
    }
}
