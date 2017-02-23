using System;

namespace Volo.Abp.EmbeddedFiles
{
    public static class EmbeddedFilePathHelper
    {
        public static string NormalizePath(string fullPath)
        {
            var fileName = fullPath;
            var extension = "";

            if (fileName.Contains("."))
            {
                extension = fullPath.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                if (extension.Contains("/"))
                {
                    //That means the file does not have extension, but a directory has "." char. So, clear extension.
                    extension = "";
                }
                else
                {
                    fileName = fullPath.Substring(0, fullPath.Length - extension.Length);
                }
            }

            return NormalizeChars(fileName) + extension;
        }
        
        private static string NormalizeChars(string fileName)
        {
            return fileName.Replace(".", "/").Replace("-", "_");
        }
    }
}