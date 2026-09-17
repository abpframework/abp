using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Implemented by a read stream (like the decrypting stream) that can verify it was
/// read to an authenticated end. The content pipeline calls it when the composed
/// stream returned by <c>GetAsync</c> reaches EOF, so a contributor stopping before
/// the end can not hide a truncation. A stream that wraps such a stream (for example
/// a custom <c>CreateDecryptingStreamAsync</c> override) should implement this
/// interface too and forward the calls to the wrapped stream, or the end verification
/// is skipped for pipeline reads.
/// </summary>
public interface IBlobAuthenticatedEndStream
{
    /// <summary>
    /// Throws if the stream has not been consumed up to its authenticated end.
    /// </summary>
    void EnsureReadToAuthenticatedEnd();

    /// <summary>
    /// Throws if the stream has not been consumed up to its authenticated end.
    /// </summary>
    ValueTask EnsureReadToAuthenticatedEndAsync(CancellationToken cancellationToken = default);
}
