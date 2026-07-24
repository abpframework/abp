using System;
using System.IO;
using System.Security.Cryptography;
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
    private readonly byte[] _associatedData;
    private readonly byte[] _keyBytes;
    private readonly IDisposable _chunkCipher;
    private readonly byte[] _nonce;
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
        // One reusable cipher and buffer each; only the trailing chunk index changes per chunk
        _associatedData = BlobEncryptionCodec.CreateReusableAssociatedData(associatedDataPrefix);
        _keyBytes = keyBytes;
        _chunkCipher = BlobEncryptionCodec.CreateChunkCipher(keyBytes);
        _nonce = BlobEncryptionCodec.CreateReusableChunkNonce(baseNonce);
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
            SetChunkIndex(_chunkIndex);
            return BlobEncryptionCodec.CreateTerminalRecordCore(_chunkCipher, _associatedData, _nonce);
        }

        SetChunkIndex(_chunkIndex);
        var chunkBytes = BlobEncryptionCodec.EncryptChunkCore(_chunkCipher, _associatedData, _nonce, plainChunk, plainChunk.Length);
        _chunkIndex++;
        return chunkBytes;
    }

    private void SetChunkIndex(int chunkIndex)
    {
        BlobEncryptionCodec.WriteChunkIndex(_nonce, chunkIndex);
        BlobEncryptionCodec.WriteChunkIndex(_associatedData, chunkIndex);
    }

    protected override void Dispose(bool disposing)
    {
        // Do not dispose the plain stream; it is owned by the caller.
        if (disposing)
        {
            _chunkCipher.Dispose();
            ClearKeyBytes();
        }

        base.Dispose(disposing);
    }

    private void ClearKeyBytes()
    {
#if NETSTANDARD2_0
        Array.Clear(_keyBytes, 0, _keyBytes.Length);
#else
        CryptographicOperations.ZeroMemory(_keyBytes);
#endif
    }
}
