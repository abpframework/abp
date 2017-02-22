using JetBrains.Annotations;

namespace Volo.Abp.EmbeddedFiles
{
    /// <summary>
    /// Provides infrastructure to use any type of files embedded into assemblies.
    /// </summary>
    public interface IEmbeddedFileManager
    {
        /// <summary>
        /// Used to get an embedded file.
        /// Can return null if file not found!
        /// </summary>
        /// <param name="fullResourcePath">Full path of the file</param>
        /// <returns>The resource</returns>
        [CanBeNull]
        EmbeddedFileInfo FindFile([NotNull] string fullResourcePath);
    }
}