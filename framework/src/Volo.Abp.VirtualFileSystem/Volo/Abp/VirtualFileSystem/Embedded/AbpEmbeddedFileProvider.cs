using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem.Embedded;

public class AbpEmbeddedFileProvider : DictionaryBasedFileProvider
{
    [NotNull]
    public Assembly Assembly { get; }

    public string? BaseNamespace { get; }

    protected override IDictionary<string, IFileInfo> Files => _files.Value;
    private readonly Lazy<Dictionary<string, IFileInfo>> _files;

    public AbpEmbeddedFileProvider(
        [NotNull] Assembly assembly,
        string? baseNamespace = null)
    {
        Check.NotNull(assembly, nameof(assembly));

        Assembly = assembly;
        BaseNamespace = baseNamespace;

        _files = new Lazy<Dictionary<string, IFileInfo>>(
            CreateFiles,
            true
        );
    }

    public void AddFiles(Dictionary<string, IFileInfo> files)
    {
        var lastModificationTime = GetLastModificationTime();

        foreach (var resourcePath in Assembly.GetManifestResourceNames())
        {
            if (!BaseNamespace.IsNullOrEmpty() && !resourcePath.StartsWith(BaseNamespace!))
            {
                continue;
            }

            var fullPath = ConvertToRelativePath(resourcePath).EnsureStartsWith('/');

            if (fullPath.Contains("/"))
            {
                AddDirectoriesRecursively(files, fullPath.Substring(0, fullPath.LastIndexOf('/')), lastModificationTime);
            }

            files[fullPath] = new EmbeddedResourceFileInfo(
                Assembly,
                resourcePath,
                fullPath,
                CalculateFileName(fullPath),
                lastModificationTime
            );
        }
    }

    private static void AddDirectoriesRecursively(Dictionary<string, IFileInfo> files, string directoryPath, DateTimeOffset lastModificationTime)
    {
        if (files.ContainsKey(directoryPath))
        {
            return;
        }

        files[directoryPath] = new VirtualDirectoryFileInfo(
            directoryPath,
            CalculateFileName(directoryPath),
            lastModificationTime
        );

        if (directoryPath.Contains("/"))
        {
            AddDirectoriesRecursively(files, directoryPath.Substring(0, directoryPath.LastIndexOf('/')), lastModificationTime);
        }
    }

    private DateTimeOffset GetLastModificationTime()
    {
        var lastModified = DateTimeOffset.UtcNow;

        if (!string.IsNullOrEmpty(Assembly.Location))
        {
            try
            {
                lastModified = File.GetLastWriteTimeUtc(Assembly.Location);
            }
            catch (PathTooLongException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        return lastModified;
    }

    private string ConvertToRelativePath(string resourceName)
    {
        if (!BaseNamespace.IsNullOrEmpty())
        {
            resourceName = resourceName.Substring(BaseNamespace!.Length + 1);
        }
        else
        {
            // Fix NET 10 RC 1 Microsoft.Extensions.FileProviders.Embedded issue temporarily
            //https://github.com/dotnet/aspnetcore/issues/63719
            string[] webContentFolders = ["wwwroot", "Pages", "Views", "Themes", "Components"];
            foreach (var contentFolder in webContentFolders.Where(contentFolder => resourceName.Contains($".{contentFolder}.")))
            {
                var index = resourceName.IndexOf(contentFolder, StringComparison.CurrentCultureIgnoreCase);
                if (index > 0)
                {
                    resourceName = resourceName.Substring(index);
                }
                break;
            }
        }

        var pathParts = resourceName.Split('.');
        if (pathParts.Length <= 2)
        {
            return resourceName;
        }

        if (pathParts.Length >= 4 && (pathParts[pathParts.Length - 2] == "min" || pathParts[pathParts.Length - 2] == "rtl"))
        {
            // Fix NET 10 RC 1 Microsoft.Extensions.FileProviders.Embedded issue temporarily
            //https://github.com/dotnet/aspnetcore/issues/63719
            pathParts = pathParts[pathParts.Length - 3] == "bundle"
                ? pathParts.Take(pathParts.Length - 4).Concat([pathParts.Skip(pathParts.Length - 4).JoinAsString(".")]).ToArray()
                : pathParts.Take(pathParts.Length - 3).Concat([pathParts.Skip(pathParts.Length - 3).JoinAsString(".")]).ToArray();

            if (pathParts.Length <= 2)
            {
                return resourceName;
            }

            var folder = pathParts.Take(pathParts.Length - 1).JoinAsString("/").Replace("_", "-");
            var fileName = pathParts[pathParts.Length - 1].Replace("_", "-");
            return folder + "/" + fileName;
        }
        else
        {
            if (pathParts.Length <= 2)
            {
                return resourceName;
            }

            var folder = pathParts.Take(pathParts.Length - 2).JoinAsString("/");
            var fileName = pathParts[pathParts.Length - 2] + "." + pathParts[pathParts.Length - 1];

            return folder + "/" + fileName;
        }
    }

    private static string CalculateFileName(string filePath)
    {
        if (!filePath.Contains("/"))
        {
            return filePath;
        }

        return filePath.Substring(filePath.LastIndexOf("/", StringComparison.Ordinal) + 1);
    }

    protected override string NormalizePath(string subpath)
    {
        return VirtualFilePathHelper.NormalizePath(subpath);
    }

    private Dictionary<string, IFileInfo> CreateFiles()
    {
        var files = new Dictionary<string, IFileInfo>(StringComparer.OrdinalIgnoreCase);
        AddFiles(files);
        return files;
    }
}
