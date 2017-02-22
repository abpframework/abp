using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.EmbeddedFiles
{
    /// <summary>
    /// Stores needed information of an embedded resource.
    /// </summary>
    public class EmbeddedFileInfo
    {
        /// <summary>
        /// File name including extension.
        /// </summary>
        [NotNull]
        public string FileName { get; }

        /// <summary>
        /// File extension (without dot) if available.
        /// </summary>
        [CanBeNull]
        public string FileExtension { get; }

        /// <summary>
        /// Content of the file.
        /// </summary>
        [NotNull]
        public byte[] Content { get; }

        /// <summary>
        /// The assembly that contains the file.
        /// </summary>
        [NotNull]
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Last modification time of the file.
        /// </summary>
        public DateTime LastModifiedUtc { get; }

        internal EmbeddedFileInfo([NotNull] string fileName, [NotNull] byte[] content, [NotNull] Assembly assembly)
        {
            Check.NotNull(fileName, nameof(fileName));
            Check.NotNull(content, nameof(content));
            Check.NotNull(assembly, nameof(assembly));

            FileName = fileName;
            Content = content;
            Assembly = assembly;

            FileExtension = CalculateFileExtension(FileName);
            LastModifiedUtc = Assembly.Location != null
                ? new FileInfo(Assembly.Location).LastWriteTimeUtc
                : DateTime.UtcNow;
        }

        private static string CalculateFileExtension(string fileName)
        {
            if (!fileName.Contains("."))
            {
                return null;
            }

            return fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }
    }
}