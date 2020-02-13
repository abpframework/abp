using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static async Task<string> ReadAllTextAsync(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                var result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
                return result;
            }
        }

        //TODO: ReadAllLinesAsync
    }
}