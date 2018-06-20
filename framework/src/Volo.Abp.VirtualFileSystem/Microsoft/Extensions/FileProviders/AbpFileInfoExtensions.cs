using System.IO;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp;

namespace Microsoft.Extensions.FileProviders
{
    public static class AbpFileInfoExtensions
    {
        /// <summary>
        /// Reads file content as string using <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        public static string ReadAsString([NotNull] this IFileInfo fileInfo)
        {
            return fileInfo.ReadAsString(Encoding.UTF8);
        }

        /// <summary>
        /// Reads file content as string using the given <paramref name="encoding"/>.
        /// </summary>
        public static string ReadAsString([NotNull] this IFileInfo fileInfo, Encoding encoding)
        {
            Check.NotNull(fileInfo, nameof(fileInfo));

            using (var stream = fileInfo.CreateReadStream())
            {
                return encoding.GetString(stream.GetAllBytes());
            }
        }
    }
}
