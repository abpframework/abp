using System.IO;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Encrypts and decrypts BLOB streams with authenticated encryption
/// (see <see cref="Volo.Abp.Security.Encryption.IByteArrayEncryptionService"/>).
/// Memory usage is constant, independent from the BLOB size.
/// </summary>
public interface IBlobEncryptionService
{
    /// <summary>
    /// Wraps the given stream so that the content read from it is encrypted.
    /// The returned stream starts with a small header (magic bytes and format version,
    /// followed by the encryption format header), the authenticated cipher chunks,
    /// and an authenticated terminal record. When the input length is known, the returned
    /// stream exposes the exact encrypted <see cref="Stream.Length"/>.
    /// </summary>
    Stream Encrypt(Stream plainStream, string passPhrase);

    /// <summary>
    /// Wraps the given stream so that the content read from it is decrypted.
    /// If the stream does not carry the encryption header, its content is returned unchanged
    /// (assumed to be stored before encryption was enabled).
    /// </summary>
    Stream Decrypt(Stream cipherStream, string passPhrase);
}
