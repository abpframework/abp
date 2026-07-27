using System;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// The passphrase resolved for encrypting a BLOB, together with its source.
/// </summary>
public class BlobEncryptionKey
{
    /// <summary>
    /// The source the passphrase was resolved from; it is recorded in the encrypted
    /// BLOB and routes the BLOB back to the same source while decrypting.
    /// </summary>
    public BlobEncryptionKeySource Source { get; }

    /// <summary>
    /// The passphrase the encryption key of the BLOB is derived from.
    /// </summary>
    [NotNull]
    public string PassPhrase { get; }

    /// <summary>
    /// Creates the resolved key; <paramref name="source"/> must be a defined
    /// <see cref="BlobEncryptionKeySource"/> value and the passphrase non-empty.
    /// </summary>
    public BlobEncryptionKey(BlobEncryptionKeySource source, [NotNull] string passPhrase)
    {
        if (source < BlobEncryptionKeySource.Container || source > BlobEncryptionKeySource.Global)
        {
            // The source is stored in the BLOB header and validated while reading;
            // an unknown value would make the BLOB permanently unreadable.
            throw new ArgumentException($"Unknown BLOB encryption key source: {source}!", nameof(source));
        }

        Source = source;
        PassPhrase = Check.NotNullOrWhiteSpace(passPhrase, nameof(passPhrase));
    }
}
