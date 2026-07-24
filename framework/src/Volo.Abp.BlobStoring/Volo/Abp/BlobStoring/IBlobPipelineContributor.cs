using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Transforms the BLOB content stream (compression, watermarking, validation...)
/// while it is saved and read. Contributors are configured per container with
/// <see cref="BlobContainerConfiguration.PipelineContributors"/> and run in the
/// configuration order while saving and in the reverse order while reading.
/// The built-in encryption always runs after the contributors while saving (and
/// before them while reading), so contributors always work on the plain content.
/// </summary>
public interface IBlobPipelineContributor
{
    /// <summary>
    /// Transform the content by replacing <see cref="BlobPipelineContext.BlobStream"/>:
    /// with a lazily transforming read-only wrapper (best for large content), or with an
    /// eagerly materialized stream. A replacement must leave the stream it received open:
    /// every stream <b>assigned</b> to <see cref="BlobPipelineContext.BlobStream"/> is disposed
    /// after the save, while the original stream stays owned by the caller. A stream is only
    /// tracked from the moment it is assigned, so if you create a stream and then do work
    /// that may fail before assigning it, dispose it yourself on the failure path.
    /// <para>
    /// Not replacing the stream is only valid for a contributor that does not consume the
    /// content (for example a metadata check). A contributor that reads the content to
    /// validate it must return a pass-through wrapper that validates the bytes as they
    /// flow (or an eagerly materialized replacement) — reading the content without
    /// replacing the stream would leave an empty/truncated stream for the provider.
    /// </para>
    /// </summary>
    Task OnSavingAsync([NotNull] BlobPipelineContext context);

    /// <summary>
    /// Reverse the save-time transformation by replacing
    /// <see cref="BlobPipelineContext.BlobStream"/> the same way. Here a replacement
    /// must dispose the stream it received when it is disposed, since the composed
    /// stream is returned to the caller as a whole.
    /// </summary>
    Task OnGettingAsync([NotNull] BlobPipelineContext context);
}
