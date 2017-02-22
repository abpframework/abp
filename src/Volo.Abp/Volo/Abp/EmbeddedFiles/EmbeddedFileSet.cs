using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.ExtensionMethods;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.EmbeddedFiles
{
    public class EmbeddedFileSet
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

        internal void AddResources(Dictionary<string, EmbeddedFileInfo> resources)
        {
            foreach (var resourceName in Assembly.GetManifestResourceNames())
            {
                if (!resourceName.StartsWith(ResourceNamespace))
                {
                    continue;
                }

                using (var stream = Assembly.GetManifestResourceStream(resourceName))
                {
                    var filePath = RootPath + ConvertToRelativePath(resourceName);

                    resources[filePath] = new EmbeddedFileInfo(
                        CalculateFileName(filePath),
                        stream.GetAllBytes(),
                        Assembly
                    );
                }
            }
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