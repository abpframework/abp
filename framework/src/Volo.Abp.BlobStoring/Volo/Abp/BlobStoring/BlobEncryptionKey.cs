using System;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// The passphrase resolved for encrypting a BLOB, together with its source.
/// </summary>
public sealed class BlobEncryptionKey
{
    public BlobEncryptionKeySource Source { get; }

    [NotNull]
    public string PassPhrase { get; }

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
