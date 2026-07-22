using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Decrypts the cipher stream chunk by chunk while being read.
/// </summary>
internal class ChunkedDecryptingReadStream : ChunkedCryptoReadStream
{
    private readonly Stream _cipherStream;
    private readonly byte[] _associatedDataPrefix;
    private readonly byte[] _keyBytes;
    private readonly byte[] _baseNonce;
    private readonly int _chunkSize;
    private int _chunkIndex;
    private bool _disposed;

    public ChunkedDecryptingReadStream(
        Stream cipherStream,
        byte[] associatedDataPrefix,
        byte[] keyBytes,
        byte[] baseNonce,
        int chunkSize)
    {
        _cipherStream = cipherStream;
        _associatedDataPrefix = associatedDataPrefix;
        _keyBytes = keyBytes;
        _baseNonce = baseNonce;
        _chunkSize = chunkSize;
    }

    protected override byte[]? ProduceNext()
    {
        var cipherChunkSize = BlobEncryptionCodec.GetCipherChunkSize(
            BlobEncryptionCodec.ReadUpTo(_cipherStream, BlobEncryptionCodec.ChunkLengthPrefixSize),
            _chunkSize
        );
        if (cipherChunkSize == 0)
        {
            var terminalTag = BlobEncryptionCodec.ReadExactly(_cipherStream, BlobEncryptionCodec.GcmTagSize);
            if (terminalTag == null || BlobEncryptionCodec.ReadUpTo(_cipherStream, 1).Length != 0)
            {
                throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid terminal record!");
            }

            BlobEncryptionCodec.VerifyTerminalRecord(_keyBytes, _associatedDataPrefix, _baseNonce, _chunkIndex, terminalTag);
            return null;
        }

        return DecryptPayload(
            BlobEncryptionCodec.ReadExactly(_cipherStream, cipherChunkSize),
            BlobEncryptionCodec.ReadExactly(_cipherStream, BlobEncryptionCodec.GcmTagSize)
        );
    }

    protected override async Task<byte[]?> ProduceNextAsync(CancellationToken cancellationToken)
    {
        var cipherChunkSize = BlobEncryptionCodec.GetCipherChunkSize(
            await BlobEncryptionCodec.ReadUpToAsync(_cipherStream, BlobEncryptionCodec.ChunkLengthPrefixSize, cancellationToken),
            _chunkSize
        );
        if (cipherChunkSize == 0)
        {
            var terminalTag = await BlobEncryptionCodec.ReadExactlyAsync(_cipherStream, BlobEncryptionCodec.GcmTagSize, cancellationToken);
            if (terminalTag == null || (await BlobEncryptionCodec.ReadUpToAsync(_cipherStream, 1, cancellationToken)).Length != 0)
            {
                throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid terminal record!");
            }

            BlobEncryptionCodec.VerifyTerminalRecord(_keyBytes, _associatedDataPrefix, _baseNonce, _chunkIndex, terminalTag);
            return null;
        }

        return DecryptPayload(
            await BlobEncryptionCodec.ReadExactlyAsync(_cipherStream, cipherChunkSize, cancellationToken),
            await BlobEncryptionCodec.ReadExactlyAsync(_cipherStream, BlobEncryptionCodec.GcmTagSize, cancellationToken)
        );
    }

    private byte[] DecryptPayload(byte[]? cipherChunk, byte[]? tag)
    {
        if (cipherChunk == null || tag == null)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: truncated chunk!");
        }

        var plainChunk = BlobEncryptionCodec.DecryptChunk(_keyBytes, _associatedDataPrefix, _baseNonce, _chunkIndex, cipherChunk, tag);
        _chunkIndex++;
        return plainChunk;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
            ClearKeyBytes();
            try
            {
                _cipherStream.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }

            return;
        }

        base.Dispose(disposing);
    }

#if !NETSTANDARD2_0
    public override async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            _disposed = true;
            ClearKeyBytes();
            try
            {
                await _cipherStream.DisposeAsync();
            }
            finally
            {
                await base.DisposeAsync();
            }

            return;
        }

        await base.DisposeAsync();
    }
#endif

    private void ClearKeyBytes()
    {
#if NETSTANDARD2_0
        Array.Clear(_keyBytes, 0, _keyBytes.Length);
#else
        System.Security.Cryptography.CryptographicOperations.ZeroMemory(_keyBytes);
#endif
    }
}
