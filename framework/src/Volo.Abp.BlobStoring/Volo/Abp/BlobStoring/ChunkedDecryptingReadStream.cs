using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Decrypts the cipher stream chunk by chunk while being read. It is the only stream
/// with an authenticated terminal record, so it is the one implementing
/// <see cref="IBlobAuthenticatedEndStream"/>.
/// </summary>
internal class ChunkedDecryptingReadStream : ChunkedCryptoReadStream, IBlobAuthenticatedEndStream
{
    private readonly Stream _cipherStream;
    private readonly byte[] _associatedData;
    private readonly byte[] _keyBytes;
    private readonly IDisposable _chunkCipher;
    private readonly byte[] _nonce;
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
        // One reusable cipher and buffer each; only the trailing chunk index changes per chunk
        _associatedData = BlobEncryptionCodec.CreateReusableAssociatedData(associatedDataPrefix);
        _keyBytes = keyBytes;
        _chunkCipher = BlobEncryptionCodec.CreateChunkCipher(keyBytes);
        _nonce = BlobEncryptionCodec.CreateReusableChunkNonce(baseNonce);
        _chunkSize = chunkSize;
    }

    public void EnsureReadToAuthenticatedEnd()
    {
        EnsureReadToAuthenticatedEndCore();
    }

    public ValueTask EnsureReadToAuthenticatedEndAsync(CancellationToken cancellationToken = default)
    {
        return EnsureReadToAuthenticatedEndCoreAsync(cancellationToken);
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

            SetChunkIndex(_chunkIndex);
            BlobEncryptionCodec.VerifyTerminalRecordCore(_chunkCipher, _associatedData, _nonce, terminalTag);
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

            SetChunkIndex(_chunkIndex);
            BlobEncryptionCodec.VerifyTerminalRecordCore(_chunkCipher, _associatedData, _nonce, terminalTag);
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

        SetChunkIndex(_chunkIndex);
        var plainChunk = BlobEncryptionCodec.DecryptChunkCore(_chunkCipher, _associatedData, _nonce, cipherChunk, tag);
        _chunkIndex++;
        return plainChunk;
    }

    private void SetChunkIndex(int chunkIndex)
    {
        BlobEncryptionCodec.WriteChunkIndex(_nonce, chunkIndex);
        BlobEncryptionCodec.WriteChunkIndex(_associatedData, chunkIndex);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
            _chunkCipher.Dispose();
            ClearKeyBytes();
            try
            {
#if NETSTANDARD2_0
                _cipherStream.Dispose();
#else
                // Also covers a provider stream that only implements DisposeAsync
                AsyncHelper.RunSync(() => _cipherStream.DisposeAsync().AsTask());
#endif
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
            _chunkCipher.Dispose();
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
        CryptographicOperations.ZeroMemory(_keyBytes);
#endif
    }
}
