using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Default implementation of <see cref="IBlobEncryptionService"/>.
/// <para>
/// Inherits the authenticated (AEAD) chunked encryption from
/// <see cref="ByteArrayEncryptionService"/> and exposes it as pull-style
/// read streams, as required by the BLOB pipeline. Memory usage is constant,
/// independent from the BLOB size.
/// </para>
/// <para>
/// Format of an encrypted BLOB: 4 bytes magic ("ABPE") + 1 byte BLOB format version,
/// followed by the <see cref="ByteArrayEncryptionService"/> output
/// (its own header + authenticated chunks). BLOBs without the magic header
/// are returned as-is on decryption, so BLOBs stored before encryption was
/// enabled stay readable.
/// </para>
/// </summary>
public class BlobEncryptionService : ByteArrayEncryptionService, IBlobEncryptionService
{
    protected static readonly byte[] MagicHeader = { (byte)'A', (byte)'B', (byte)'P', (byte)'E' };

    protected const byte BlobFormatVersion = 1;

    public BlobEncryptionService(IOptions<AbpByteArrayEncryptionOptions> options)
        : base(options)
    {
    }

    public virtual Stream Encrypt(Stream plainStream, string passPhrase)
    {
        Check.NotNull(plainStream, nameof(plainStream));
        Check.NotNullOrWhiteSpace(passPhrase, nameof(passPhrase));

        if (Options.ChunkSize <= 0 || Options.ChunkSize > MaximumChunkSize)
        {
            throw new AbpException($"{nameof(Options.ChunkSize)} must be between 1 and {MaximumChunkSize} bytes!");
        }

        var algorithm = GetEncryptionAlgorithm();
        var keyBytes = DeriveKeyBytes(passPhrase, Options.DefaultSalt, algorithm);

        var baseNonce = new byte[BaseNonceSize];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(baseNonce);
        }

        var header = BuildHeader(algorithm, Options.ChunkSize, baseNonce);

        return new ChunkedEncryptingReadStream(
            this,
            plainStream,
            header,
            keyBytes,
            baseNonce,
            algorithm,
            Options.ChunkSize,
            TryCalculateEncryptedLength(plainStream, algorithm, Options.ChunkSize)
        );
    }

    public virtual Stream Decrypt(Stream cipherStream, string passPhrase)
    {
        Check.NotNull(cipherStream, nameof(cipherStream));
        Check.NotNullOrWhiteSpace(passPhrase, nameof(passPhrase));

        var prefix = ReadUpTo(cipherStream, MagicHeader.Length + 1);
        if (prefix.Length < MagicHeader.Length + 1 || !HasMagicHeader(prefix))
        {
            // Not an encrypted BLOB, return as-is for backward compatibility
            return new PrefixingReadStream(prefix, cipherStream);
        }

        if (prefix[MagicHeader.Length] != BlobFormatVersion)
        {
            throw new AbpException($"Unsupported BLOB encryption format version: {prefix[MagicHeader.Length]}!");
        }

        var header = ReadExactly(cipherStream, HeaderSize);
        if (header == null)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: missing header!");
        }

        if (header[0] != FormatVersion)
        {
            throw new AbpException($"Unsupported encryption format version: {header[0]}!");
        }

        var algorithm = header[1];
        if (algorithm != AlgorithmAesGcm && algorithm != AlgorithmAesCbcHmacSha256)
        {
            throw new AbpException($"Unsupported encryption algorithm: {algorithm}!");
        }

        var chunkSize = ReadInt32BigEndian(header, 2);
        if (chunkSize <= 0 || chunkSize > MaximumChunkSize)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid chunk size!");
        }

        var baseNonce = new byte[BaseNonceSize];
        Array.Copy(header, 6, baseNonce, 0, BaseNonceSize);

        var keyBytes = DeriveKeyBytes(passPhrase, Options.DefaultSalt, algorithm);

        return new ChunkedDecryptingReadStream(
            this,
            cipherStream,
            header,
            keyBytes,
            baseNonce,
            algorithm,
            chunkSize,
            algorithm == AlgorithmAesGcm ? GcmTagSize : HmacSize,
            algorithm == AlgorithmAesGcm ? chunkSize : chunkSize + AesBlockSize
        );
    }

    internal byte[] EncryptChunkToBytes(byte algorithm, byte[] keyBytes, byte[] header, byte[] baseNonce, int chunkIndex, byte[] plainChunk, int plainChunkLength)
    {
        using (var chunkStream = new MemoryStream())
        {
            EncryptChunk(algorithm, keyBytes, header, baseNonce, chunkIndex, plainChunk, plainChunkLength, chunkStream);
            return chunkStream.ToArray();
        }
    }

    internal byte[]? ReadExactlyCore(Stream stream, int count)
    {
        return ReadExactly(stream, count);
    }

    internal byte[] ReadUpToCore(Stream stream, int count)
    {
        return ReadUpTo(stream, count);
    }

    internal byte[] DecryptChunkCore(byte algorithm, byte[] keyBytes, byte[] header, byte[] baseNonce, int chunkIndex, byte[] cipherChunk, byte[] tag)
    {
        return DecryptChunk(algorithm, keyBytes, header, baseNonce, chunkIndex, cipherChunk, tag);
    }

    internal byte[] WriteTerminalRecordToBytes(byte algorithm, byte[] keyBytes, byte[] header, byte[] baseNonce, int chunkIndex)
    {
        using (var stream = new MemoryStream())
        {
            WriteTerminalRecord(algorithm, keyBytes, header, baseNonce, chunkIndex, stream);
            return stream.ToArray();
        }
    }

    internal void VerifyTerminalRecordCore(byte algorithm, byte[] keyBytes, byte[] header, byte[] baseNonce, int chunkIndex, byte[] tag)
    {
        VerifyTerminalRecord(algorithm, keyBytes, header, baseNonce, chunkIndex, tag);
    }

    private static long? TryCalculateEncryptedLength(Stream plainStream, byte algorithm, int chunkSize)
    {
        if (!plainStream.CanSeek)
        {
            return null;
        }

        try
        {
            var plainLength = plainStream.Length - plainStream.Position;
            if (plainLength < 0)
            {
                return null;
            }

            var tagSize = algorithm == AlgorithmAesGcm ? GcmTagSize : HmacSize;
            var fullChunkCount = plainLength / chunkSize;
            var remainder = plainLength % chunkSize;

            checked
            {
                var length = MagicHeader.Length + 1L + HeaderSize + ChunkLengthPrefixSize + tagSize;
                length += fullChunkCount * (ChunkLengthPrefixSize + tagSize + GetCipherChunkLength(algorithm, chunkSize));
                if (remainder > 0)
                {
                    length += ChunkLengthPrefixSize + tagSize + GetCipherChunkLength(algorithm, remainder);
                }

                return length;
            }
        }
        catch (NotSupportedException)
        {
            return null;
        }
        catch (OverflowException)
        {
            return null;
        }
    }

    private static long GetCipherChunkLength(byte algorithm, long plainChunkLength)
    {
        return algorithm == AlgorithmAesGcm
            ? plainChunkLength
            : ((plainChunkLength / AesBlockSize) + 1) * AesBlockSize;
    }

    private static bool HasMagicHeader(byte[] prefix)
    {
        for (var i = 0; i < MagicHeader.Length; i++)
        {
            if (prefix[i] != MagicHeader[i])
            {
                return false;
            }
        }

        return true;
    }

    private static int ReadInt32BigEndian(byte[] buffer, int offset)
    {
        return (buffer[offset] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | buffer[offset + 3];
    }

    /// <summary>
    /// A read-only, non-seekable stream that serves output produced chunk by chunk,
    /// so memory usage stays constant regardless of the total data size.
    /// </summary>
    private abstract class ChunkedCryptoReadStream : Stream
    {
        private readonly long? _length;
        private byte[]? _outputBuffer;
        private int _outputBufferPosition;
        private bool _finished;

        protected ChunkedCryptoReadStream(long? length = null)
        {
            _length = length;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => _length ?? throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            while (true)
            {
                if (_outputBuffer != null && _outputBufferPosition < _outputBuffer.Length)
                {
                    var toCopy = Math.Min(count, _outputBuffer.Length - _outputBufferPosition);
                    Array.Copy(_outputBuffer, _outputBufferPosition, buffer, offset, toCopy);
                    _outputBufferPosition += toCopy;
                    return toCopy;
                }

                if (_finished)
                {
                    return 0;
                }

                _outputBuffer = ProduceNext();
                _outputBufferPosition = 0;

                if (_outputBuffer == null)
                {
                    _finished = true;
                    return 0;
                }
            }
        }

        /// <summary>
        /// Produces the next output bytes, or null when there is no more output.
        /// </summary>
        protected abstract byte[]? ProduceNext();

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }

    private class ChunkedEncryptingReadStream : ChunkedCryptoReadStream
    {
        private readonly BlobEncryptionService _owner;
        private readonly Stream _plainStream;
        private readonly byte[] _header;
        private readonly byte[] _keyBytes;
        private readonly byte[] _baseNonce;
        private readonly byte _algorithm;
        private readonly int _chunkSize;
        private bool _terminalEmitted;
        private bool _headerEmitted;
        private int _chunkIndex;

        public ChunkedEncryptingReadStream(
            BlobEncryptionService owner,
            Stream plainStream,
            byte[] header,
            byte[] keyBytes,
            byte[] baseNonce,
            byte algorithm,
            int chunkSize,
            long? encryptedLength)
            : base(encryptedLength)
        {
            _owner = owner;
            _plainStream = plainStream;
            _header = header;
            _keyBytes = keyBytes;
            _baseNonce = baseNonce;
            _algorithm = algorithm;
            _chunkSize = chunkSize;
        }

        protected override byte[]? ProduceNext()
        {
            if (!_headerEmitted)
            {
                _headerEmitted = true;

                var prefix = new byte[MagicHeader.Length + 1 + _header.Length];
                MagicHeader.CopyTo(prefix, 0);
                prefix[MagicHeader.Length] = BlobFormatVersion;
                Array.Copy(_header, 0, prefix, MagicHeader.Length + 1, _header.Length);
                return prefix;
            }

            var plainChunk = _owner.ReadUpToCore(_plainStream, _chunkSize);
            if (plainChunk.Length == 0)
            {
                if (_terminalEmitted)
                {
                    return null;
                }

                _terminalEmitted = true;
                return _owner.WriteTerminalRecordToBytes(
                    _algorithm,
                    _keyBytes,
                    _header,
                    _baseNonce,
                    _chunkIndex
                );
            }

            return _owner.EncryptChunkToBytes(
                _algorithm,
                _keyBytes,
                _header,
                _baseNonce,
                _chunkIndex++,
                plainChunk,
                plainChunk.Length
            );
        }

        protected override void Dispose(bool disposing)
        {
            // Do not dispose the plain stream; it is owned by the caller.
        }
    }

    private class ChunkedDecryptingReadStream : ChunkedCryptoReadStream
    {
        private readonly BlobEncryptionService _owner;
        private readonly Stream _cipherStream;
        private readonly byte[] _header;
        private readonly byte[] _keyBytes;
        private readonly byte[] _baseNonce;
        private readonly byte _algorithm;
        private readonly int _tagSize;
        private readonly int _maxCipherChunkSize;
        private int _chunkIndex;

        public ChunkedDecryptingReadStream(
            BlobEncryptionService owner,
            Stream cipherStream,
            byte[] header,
            byte[] keyBytes,
            byte[] baseNonce,
            byte algorithm,
            int chunkSize,
            int tagSize,
            int maxCipherChunkSize)
        {
            _owner = owner;
            _cipherStream = cipherStream;
            _header = header;
            _keyBytes = keyBytes;
            _baseNonce = baseNonce;
            _algorithm = algorithm;
            _tagSize = tagSize;
            _maxCipherChunkSize = maxCipherChunkSize;
        }

        protected override byte[]? ProduceNext()
        {
            var lengthPrefix = _owner.ReadUpToCore(_cipherStream, 4);
            if (lengthPrefix.Length == 0)
            {
                throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: missing terminal record!");
            }

            if (lengthPrefix.Length < 4)
            {
                throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: truncated chunk!");
            }

            var cipherChunkSize = ReadInt32BigEndian(lengthPrefix, 0);
            if (cipherChunkSize == 0)
            {
                var terminalTag = _owner.ReadExactlyCore(_cipherStream, _tagSize);
                if (terminalTag == null || _owner.ReadUpToCore(_cipherStream, 1).Length != 0)
                {
                    throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid terminal record!");
                }

                _owner.VerifyTerminalRecordCore(
                    _algorithm,
                    _keyBytes,
                    _header,
                    _baseNonce,
                    _chunkIndex,
                    terminalTag
                );
                return null;
            }

            if (cipherChunkSize < 0 || cipherChunkSize > _maxCipherChunkSize)
            {
                throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid chunk length!");
            }

            var cipherChunk = _owner.ReadExactlyCore(_cipherStream, cipherChunkSize);
            var tag = _owner.ReadExactlyCore(_cipherStream, _tagSize);
            if (cipherChunk == null || tag == null)
            {
                throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: truncated chunk!");
            }

            return _owner.DecryptChunkCore(
                _algorithm,
                _keyBytes,
                _header,
                _baseNonce,
                _chunkIndex++,
                cipherChunk,
                tag
            );
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cipherStream.Dispose();
            }
        }
    }

    private sealed class PrefixingReadStream : Stream
    {
        private readonly byte[] _prefix;
        private readonly Stream _stream;
        private int _prefixPosition;

        public PrefixingReadStream(byte[] prefix, Stream stream)
        {
            _prefix = prefix;
            _stream = stream;
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_prefixPosition < _prefix.Length)
            {
                var readCount = Math.Min(count, _prefix.Length - _prefixPosition);
                Array.Copy(_prefix, _prefixPosition, buffer, offset, readCount);
                _prefixPosition += readCount;
                return readCount;
            }

            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
