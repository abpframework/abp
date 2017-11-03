using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.VirtualFileSystem.Embedded
{
    public class EmbeddedFileSet : IVirtualFileSet
    {
        public string RootPath { get; }

        public Assembly Assembly { get; }

        public string ResourceNamespace { get; }

        public EmbeddedFileSet(string rootPath, Assembly assembly, string resourceNamespace)
        {
            RootPath = rootPath.EnsureEndsWith('/');
            Assembly = assembly;
            ResourceNamespace = resourceNamespace;
        }

        public void AddFiles(Dictionary<string, IFileInfo> files)
        {
            var lastModificationTime = GetLastModificationTime();

            foreach (var resourcePath in Assembly.GetManifestResourceNames())
            {
                if (!resourcePath.StartsWith(ResourceNamespace))
                {
                    continue;
                }

                var fullPath = RootPath + ConvertToRelativePath(resourcePath);

                if (fullPath.Contains("/"))
                {
                    AddDirectoriesRecursively(files, fullPath.Substring(0, fullPath.LastIndexOf('/')), lastModificationTime);
                }

                files[fullPath] = new EmbeddedResourceFileInfo(
                    Assembly,
                    resourcePath,
                    fullPath,
                    CalculateFileName(fullPath),
                    lastModificationTime,
                    false
                );
            }
        }

        private void AddDirectoriesRecursively(Dictionary<string, IFileInfo> files, string directoryPath, DateTimeOffset lastModificationTime)
        {
            if (files.ContainsKey(directoryPath))
            {
                return;
            }

            files[directoryPath] = new EmbeddedResourceFileInfo( //TODO: Create a different simpler class!
                Assembly,
                null,
                directoryPath,
                CalculateFileName(directoryPath),
                lastModificationTime,
                true
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
            resourceName = resourceName.Substring(ResourceNamespace.Length + 1);

            var pathParts = resourceName.Split('.');
            if (pathParts.Length <= 2)
            {
                return resourceName;
            }

            var folder = pathParts.Take(pathParts.Length - 2).Select(NormalizeFolderName).JoinAsString("/");
            var fileName = pathParts[pathParts.Length - 2] + "." + pathParts[pathParts.Length - 1];

            return folder + "/" + fileName;
        }

        private static string NormalizeFolderName(string pathPart)
        {
            //TODO: Implement all rules of .NET
            return pathPart.Replace('-', '_');
        }

        private static string CalculateFileName(string filePath)
        {
            if (!filePath.Contains("/"))
            {
                return filePath;
            }

            return filePath.Substring(filePath.LastIndexOf("/", StringComparison.Ordinal) + 1);
        }
    }
}