using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Encrypts the source stream chunk by chunk while being read.
/// </summary>
internal class ChunkedEncryptingReadStream : ChunkedCryptoReadStream
{
    private readonly Stream _plainStream;
    private readonly byte[] _prefix;
    private readonly byte[] _associatedDataPrefix;
    private readonly byte[] _keyBytes;
    private readonly byte[] _baseNonce;
    private readonly int _chunkSize;
    private bool _prefixEmitted;
    private bool _terminalEmitted;
    private int _chunkIndex;

    public ChunkedEncryptingReadStream(
        Stream plainStream,
        byte[] prefix,
        byte[] associatedDataPrefix,
        byte[] keyBytes,
        byte[] baseNonce,
        int chunkSize,
        long? encryptedLength)
        : base(encryptedLength)
    {
        _plainStream = plainStream;
        _prefix = prefix;
        _associatedDataPrefix = associatedDataPrefix;
        _keyBytes = keyBytes;
        _baseNonce = baseNonce;
        _chunkSize = chunkSize;
    }

    protected override byte[]? ProduceNext()
    {
        var prefix = TryProducePrefix();
        if (prefix != null)
        {
            return prefix;
        }

        return ProducePayload(BlobEncryptionCodec.ReadUpTo(_plainStream, _chunkSize));
    }

    protected override async Task<byte[]?> ProduceNextAsync(CancellationToken cancellationToken)
    {
        var prefix = TryProducePrefix();
        if (prefix != null)
        {
            return prefix;
        }

        return ProducePayload(await BlobEncryptionCodec.ReadUpToAsync(_plainStream, _chunkSize, cancellationToken));
    }

    private byte[]? TryProducePrefix()
    {
        if (_prefixEmitted)
        {
            return null;
        }

        _prefixEmitted = true;
        return _prefix;
    }

    private byte[]? ProducePayload(byte[] plainChunk)
    {
        if (plainChunk.Length == 0)
        {
            if (_terminalEmitted)
            {
                return null;
            }

            _terminalEmitted = true;
            return BlobEncryptionCodec.CreateTerminalRecord(_keyBytes, _associatedDataPrefix, _baseNonce, _chunkIndex);
        }

        var chunkBytes = BlobEncryptionCodec.EncryptChunk(_keyBytes, _associatedDataPrefix, _baseNonce, _chunkIndex, plainChunk, plainChunk.Length);
        _chunkIndex++;
        return chunkBytes;
    }

    protected override void Dispose(bool disposing)
    {
        // Do not dispose the plain stream; it is owned by the caller.
        if (disposing)
        {
            ClearKeyBytes();
        }

        base.Dispose(disposing);
    }

    private void ClearKeyBytes()
    {
#if NETSTANDARD2_0
        System.Array.Clear(_keyBytes, 0, _keyBytes.Length);
#else
        System.Security.Cryptography.CryptographicOperations.ZeroMemory(_keyBytes);
#endif
    }
}
