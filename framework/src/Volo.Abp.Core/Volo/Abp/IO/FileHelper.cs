using System.IO;
using JetBrains.Annotations;

namespace Volo.Abp.IO
{
    /// <summary>
    /// A helper class for File operations.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Checks and deletes given file if it does exists.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Gets extension of a file.
        /// </summary>
        /// <param name="fileNameWithExtension"></param>
        /// <returns>
        /// Returns extension without dot.
        /// Returns null if given <paramref name="fileNameWithExtension"></paramref> does not include dot.
        /// </returns>
        [CanBeNull]
        public static string GetExtension([NotNull] string fileNameWithExtension)
        {
            Check.NotNull(fileNameWithExtension, nameof(fileNameWithExtension));

            var lastDotIndex = fileNameWithExtension.LastIndexOf('.');
            if (lastDotIndex < 0)
            {
                return null;
            }

            return fileNameWithExtension.Substring(lastDotIndex + 1);
        }
    }
}