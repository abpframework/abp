using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// A contributor to the BLOB pipeline. Contributors are executed inside the
/// <see cref="BlobContainer"/>, before/after the actual <see cref="IBlobProvider"/> call,
/// and can transform the BLOB stream (e.g. encryption, compression).
/// </summary>
public interface IBlobPipelineContributor
{
    /// <summary>
    /// Called before a BLOB is saved by the provider.
    /// Return the (possibly transformed) stream to be stored.
    /// </summary>
    Task<Stream> OnSaveAsync(BlobPipelineSaveArgs args);

    /// <summary>
    /// Called after a BLOB is read from the provider.
    /// Return the (possibly transformed) stream to be returned to the caller.
    /// </summary>
    Task<Stream> OnGetAsync(BlobPipelineGetArgs args);
}
