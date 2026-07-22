using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Implements the encrypted BLOB format (version 1) using AES-256-GCM.
/// Not available on .NET Standard 2.0 (no AES-GCM).
/// <para>
/// Format: "ABPE" magic (4) + format version (1) + header (34: algorithm 1,
/// key source 1, KDF iterations 4, random per-BLOB KDF salt 16, chunk size 4,
/// base nonce 8), followed by authenticated chunk records (4-byte big-endian
/// cipher length, cipher chunk, 16-byte tag) and an authenticated zero-length
/// terminal record. The whole prefix, the storage identity (container, BLOB
/// name, tenant) and the chunk index are bound to every chunk as associated
/// data; the per-BLOB salt gives every BLOB its own derived key.
/// </para>
/// </summary>
internal sealed class BlobEncryptionCodec : ITransientDependency
{
    internal static readonly byte[] Magic = { (byte)'A', (byte)'B', (byte)'P', (byte)'E' };

    internal const byte FormatVersion = 1;
    internal const byte AlgorithmAesGcm = 1;
    internal const int MinKdfIterations = 100_000;
    internal const int MaxKdfIterations = 600_000; // reader cap: bounded headroom above the writer constant
    internal const int KdfSaltSize = 16;
    internal const int ChunkSize = 64 * 1024;
    internal const int MaxChunkSize = 1024 * 1024; // reader cap: bounds allocations driven by the (pre-authentication) header
    internal const int BaseNonceSize = 8;
    internal const int HeaderSize = 34; // algorithm(1) + keySource(1) + iterations(4) + salt(16) + chunkSize(4) + baseNonce(8)
    internal const int ChunkLengthPrefixSize = 4;
    internal const int GcmNonceSize = 12;
    internal const int GcmTagSize = 16;

    private readonly IBlobEncryptionKeyProvider _keyProvider;
    private readonly AbpBlobStoringEncryptionOptions _options;

    public BlobEncryptionCodec(
        IBlobEncryptionKeyProvider keyProvider,
        Microsoft.Extensions.Options.IOptions<AbpBlobStoringEncryptionOptions> options)
    {
        _keyProvider = keyProvider;
        _options = options.Value;
    }

    // The key is fully resolved before the stream is returned, so the resolution scope can be released.
    public async Task<Stream> CreateEncryptingStreamAsync(
        BlobContainerConfiguration configuration,
        string containerName,
        string blobName,
        Guid? tenantId,
        Stream plainStream,
        CancellationToken cancellationToken = default)
    {
#if NETSTANDARD2_0
        // Fail before any output is produced, so no partial (corrupted) data is ever written.
        throw new PlatformNotSupportedException("BLOB encryption requires AES-GCM, which is not available on .NET Standard 2.0!");
#else
#if NET8_0_OR_GREATER
        if (!AesGcm.IsSupported)
        {
            throw new PlatformNotSupportedException("AES-GCM is not supported on this platform!");
        }
#endif
        var kdfIterations = _options.KdfIterations;
        if (kdfIterations < MinKdfIterations || kdfIterations > MaxKdfIterations)
        {
            throw new AbpException(
                $"{nameof(AbpBlobStoringEncryptionOptions)}.{nameof(AbpBlobStoringEncryptionOptions.KdfIterations)} " +
                $"must be between {MinKdfIterations} and {MaxKdfIterations}!");
        }

        var key = await _keyProvider.ResolveForEncryptionAsync(configuration, cancellationToken);

        var salt = new byte[KdfSaltSize];
        var baseNonce = new byte[BaseNonceSize];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(salt);
            random.GetBytes(baseNonce);
        }

        var header = BuildHeader(key.Source, kdfIterations, salt, ChunkSize, baseNonce);
        var blobPrefix = CreateBlobPrefix(header);
        cancellationToken.ThrowIfCancellationRequested();
        var keyBytes = DeriveKeyBytes(key.PassPhrase, salt, kdfIterations);

        return new ChunkedEncryptingReadStream(
            plainStream,
            blobPrefix,
            BuildAssociatedDataPrefix(blobPrefix, containerName, blobName, tenantId),
            keyBytes,
            baseNonce,
            ChunkSize,
            TryCalculateEncryptedLength(plainStream, ChunkSize)
        );
#endif
    }

    public async Task<Stream> CreateDecryptingStreamAsync(
        BlobContainerConfiguration configuration,
        string containerName,
        string blobName,
        Guid? tenantId,
        Stream cipherStream,
        CancellationToken cancellationToken = default)
    {
        var prefix = await ReadUpToAsync(cipherStream, Magic.Length + 1, cancellationToken);
        if (!HasMagic(prefix))
        {
            if (BlobEncryptionConfiguration.IsLegacyPlaintextAllowed(configuration))
            {
                return new PrefixingReadStream(prefix, cipherStream);
            }

            throw new AbpException(
                "The BLOB does not have the encrypted BLOB format. If it was stored before encryption " +
                "was enabled for the container, enable reading legacy plaintext BLOBs explicitly " +
                "(see the UseEncryption extension method). Otherwise the BLOB is corrupted or tampered."
            );
        }

        if (prefix[Magic.Length] != FormatVersion)
        {
            throw new AbpException($"Unsupported encrypted BLOB format version: {prefix[Magic.Length]}!");
        }

#if NETSTANDARD2_0
        throw new PlatformNotSupportedException("BLOB decryption requires AES-GCM, which is not available on .NET Standard 2.0!");
#else
#if NET8_0_OR_GREATER
        if (!AesGcm.IsSupported)
        {
            throw new PlatformNotSupportedException("AES-GCM is not supported on this platform!");
        }
#endif
        var header = await ReadExactlyAsync(cipherStream, HeaderSize, cancellationToken);
        if (header == null)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: missing header!");
        }

        if (header[0] != AlgorithmAesGcm)
        {
            throw new AbpException($"Unsupported encrypted BLOB algorithm: {header[0]}!");
        }

        var keySource = header[1];
        if (keySource < (byte)BlobEncryptionKeySource.Container || keySource > (byte)BlobEncryptionKeySource.Global)
        {
            throw new AbpException($"Unknown BLOB encryption key source: {keySource}!");
        }

        var iterations = ReadInt32BigEndian(header, 2);
        if (iterations <= 0 || iterations > MaxKdfIterations)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid KDF iteration count!");
        }

        var salt = new byte[KdfSaltSize];
        Array.Copy(header, 6, salt, 0, KdfSaltSize);

        var chunkSize = ReadInt32BigEndian(header, 22);
        if (chunkSize <= 0 || chunkSize > MaxChunkSize)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid chunk size!");
        }

        var baseNonce = new byte[BaseNonceSize];
        Array.Copy(header, 26, baseNonce, 0, BaseNonceSize);

        var passPhrase = await _keyProvider.ResolveForDecryptionAsync(
            (BlobEncryptionKeySource)keySource,
            configuration,
            cancellationToken
        );
        cancellationToken.ThrowIfCancellationRequested();
        var keyBytes = DeriveKeyBytes(passPhrase, salt, iterations);

        var blobPrefix = new byte[Magic.Length + 1 + HeaderSize];
        Array.Copy(prefix, 0, blobPrefix, 0, Magic.Length + 1);
        Array.Copy(header, 0, blobPrefix, Magic.Length + 1, HeaderSize);

        return new ChunkedDecryptingReadStream(
            cipherStream,
            BuildAssociatedDataPrefix(blobPrefix, containerName, blobName, tenantId),
            keyBytes,
            baseNonce,
            chunkSize
        );
#endif
    }

    internal static byte[] BuildHeader(BlobEncryptionKeySource keySource, int iterations, byte[] salt, int chunkSize, byte[] baseNonce)
    {
        var header = new byte[HeaderSize];
        header[0] = AlgorithmAesGcm;
        header[1] = (byte)keySource;
        WriteInt32BigEndian(header, 2, iterations);
        Array.Copy(salt, 0, header, 6, KdfSaltSize);
        WriteInt32BigEndian(header, 22, chunkSize);
        Array.Copy(baseNonce, 0, header, 26, BaseNonceSize);
        return header;
    }

    // Length-prefixed identity fields: a validly encrypted BLOB can not be read
    // from another BLOB name, container or tenant.
    internal static byte[] BuildAssociatedDataPrefix(byte[] blobPrefix, string containerName, string blobName, Guid? tenantId)
    {
        var containerNameBytes = Encoding.UTF8.GetBytes(containerName);
        var blobNameBytes = Encoding.UTF8.GetBytes(blobName);
        var tenantIdBytes = tenantId?.ToByteArray() ?? Array.Empty<byte>();

        var prefix = new byte[blobPrefix.Length + 4 + containerNameBytes.Length + 4 + blobNameBytes.Length + 4 + tenantIdBytes.Length];
        var offset = 0;

        Array.Copy(blobPrefix, 0, prefix, offset, blobPrefix.Length);
        offset += blobPrefix.Length;

        offset = WriteLengthPrefixed(prefix, offset, containerNameBytes);
        offset = WriteLengthPrefixed(prefix, offset, blobNameBytes);
        WriteLengthPrefixed(prefix, offset, tenantIdBytes);

        return prefix;
    }

    private static int WriteLengthPrefixed(byte[] buffer, int offset, byte[] bytes)
    {
        WriteInt32BigEndian(buffer, offset, bytes.Length);
        Array.Copy(bytes, 0, buffer, offset + 4, bytes.Length);
        return offset + 4 + bytes.Length;
    }

    internal static byte[] CreateBlobPrefix(byte[] header)
    {
        var prefix = new byte[Magic.Length + 1 + header.Length];
        Magic.CopyTo(prefix, 0);
        prefix[Magic.Length] = FormatVersion;
        Array.Copy(header, 0, prefix, Magic.Length + 1, header.Length);
        return prefix;
    }

    internal static byte[] DeriveKeyBytes(string passPhrase, byte[] salt, int iterations)
    {
#if NETSTANDARD2_0
        throw new PlatformNotSupportedException("BLOB encryption requires AES-GCM, which is not available on .NET Standard 2.0!");
#elif NET8_0_OR_GREATER
        return Rfc2898DeriveBytes.Pbkdf2(passPhrase, salt, iterations, HashAlgorithmName.SHA256, 32);
#else
        using var password = new Rfc2898DeriveBytes(passPhrase, salt, iterations, HashAlgorithmName.SHA256);
        return password.GetBytes(32);
#endif
    }

    internal static byte[] EncryptChunk(byte[] keyBytes, byte[] associatedDataPrefix, byte[] baseNonce, int chunkIndex, byte[] plainChunk, int plainChunkLength)
    {
#if NETSTANDARD2_0
        throw new PlatformNotSupportedException("AES-GCM is not available on .NET Standard 2.0!");
#else
        var record = new byte[ChunkLengthPrefixSize + plainChunkLength + GcmTagSize];
        WriteInt32BigEndian(record, 0, plainChunkLength);

        using (var aesGcm = CreateAesGcm(keyBytes))
        {
            aesGcm.Encrypt(
                CreateChunkNonce(baseNonce, chunkIndex),
                plainChunk.AsSpan(0, plainChunkLength),
                record.AsSpan(ChunkLengthPrefixSize, plainChunkLength),
                record.AsSpan(ChunkLengthPrefixSize + plainChunkLength, GcmTagSize),
                CreateChunkAssociatedData(associatedDataPrefix, chunkIndex)
            );
        }

        return record;
#endif
    }

    internal static byte[] DecryptChunk(byte[] keyBytes, byte[] associatedDataPrefix, byte[] baseNonce, int chunkIndex, byte[] cipherChunk, byte[] tag)
    {
#if NETSTANDARD2_0
        throw new PlatformNotSupportedException("AES-GCM is not available on .NET Standard 2.0!");
#else
        var plainChunk = new byte[cipherChunk.Length];
        using (var aesGcm = CreateAesGcm(keyBytes))
        {
            // Throws CryptographicException if the authentication tag is invalid.
            aesGcm.Decrypt(CreateChunkNonce(baseNonce, chunkIndex), cipherChunk, tag, plainChunk, CreateChunkAssociatedData(associatedDataPrefix, chunkIndex));
        }

        return plainChunk;
#endif
    }

    // The authenticated terminal record makes truncation of complete chunks detectable
    internal static byte[] CreateTerminalRecord(byte[] keyBytes, byte[] associatedDataPrefix, byte[] baseNonce, int chunkIndex)
    {
#if NETSTANDARD2_0
        throw new PlatformNotSupportedException("AES-GCM is not available on .NET Standard 2.0!");
#else
        var record = new byte[ChunkLengthPrefixSize + GcmTagSize];
        using (var aesGcm = CreateAesGcm(keyBytes))
        {
            aesGcm.Encrypt(
                CreateChunkNonce(baseNonce, chunkIndex),
                Array.Empty<byte>(),
                Array.Empty<byte>(),
                record.AsSpan(ChunkLengthPrefixSize, GcmTagSize),
                CreateChunkAssociatedData(associatedDataPrefix, chunkIndex)
            );
        }

        return record;
#endif
    }

    internal static void VerifyTerminalRecord(byte[] keyBytes, byte[] associatedDataPrefix, byte[] baseNonce, int chunkIndex, byte[] tag)
    {
#if NETSTANDARD2_0
        throw new PlatformNotSupportedException("AES-GCM is not available on .NET Standard 2.0!");
#else
        using (var aesGcm = CreateAesGcm(keyBytes))
        {
            // Throws CryptographicException if the tag is invalid.
            aesGcm.Decrypt(
                CreateChunkNonce(baseNonce, chunkIndex),
                Array.Empty<byte>(),
                tag,
                Array.Empty<byte>(),
                CreateChunkAssociatedData(associatedDataPrefix, chunkIndex)
            );
        }
#endif
    }

    // Nonce = 8-byte random base + 4-byte chunk index; the per-BLOB key (random salt)
    // makes cross-BLOB reuse harmless and the index keeps it unique within the BLOB.
    internal static byte[] CreateChunkNonce(byte[] baseNonce, int chunkIndex)
    {
        if (chunkIndex < 0)
        {
            // A wrapped chunk index would repeat a nonce for the same key, which breaks AES-GCM.
            throw new AbpException("The data is too large: the maximum chunk count has been exceeded!");
        }

        var nonce = new byte[GcmNonceSize];
        Array.Copy(baseNonce, 0, nonce, 0, BaseNonceSize);
        WriteInt32BigEndian(nonce, BaseNonceSize, chunkIndex);
        return nonce;
    }

    internal static byte[] CreateChunkAssociatedData(byte[] associatedDataPrefix, int chunkIndex)
    {
        var associatedData = new byte[associatedDataPrefix.Length + 4];
        Array.Copy(associatedDataPrefix, 0, associatedData, 0, associatedDataPrefix.Length);
        WriteInt32BigEndian(associatedData, associatedDataPrefix.Length, chunkIndex);
        return associatedData;
    }

    internal static int GetCipherChunkSize(byte[] lengthPrefix, int maxCipherChunkSize)
    {
        if (lengthPrefix.Length == 0)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: missing terminal record!");
        }

        if (lengthPrefix.Length < ChunkLengthPrefixSize)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: truncated chunk!");
        }

        var cipherChunkSize = ReadInt32BigEndian(lengthPrefix, 0);
        if (cipherChunkSize < 0 || cipherChunkSize > maxCipherChunkSize)
        {
            throw new AbpException("The encrypted BLOB is corrupted or has an invalid format: invalid chunk length!");
        }

        return cipherChunkSize;
    }

    internal static byte[]? ReadExactly(Stream stream, int count)
    {
        var buffer = ReadUpTo(stream, count);
        return buffer.Length == count ? buffer : null;
    }

    internal static byte[] ReadUpTo(Stream stream, int count)
    {
        var buffer = new byte[count];
        var totalReadCount = 0;
        while (totalReadCount < count)
        {
            var readCount = stream.Read(buffer, totalReadCount, count - totalReadCount);
            if (readCount == 0)
            {
                break;
            }

            totalReadCount += readCount;
        }

        if (totalReadCount == count)
        {
            return buffer;
        }

        var result = new byte[totalReadCount];
        Array.Copy(buffer, 0, result, 0, totalReadCount);
        return result;
    }

    internal static async Task<byte[]?> ReadExactlyAsync(Stream stream, int count, CancellationToken cancellationToken = default)
    {
        var buffer = await ReadUpToAsync(stream, count, cancellationToken);
        return buffer.Length == count ? buffer : null;
    }

    internal static async Task<byte[]> ReadUpToAsync(Stream stream, int count, CancellationToken cancellationToken = default)
    {
        var buffer = new byte[count];
        var totalReadCount = 0;
        while (totalReadCount < count)
        {
            var readCount = await stream.ReadAsync(buffer, totalReadCount, count - totalReadCount, cancellationToken);
            if (readCount == 0)
            {
                break;
            }

            totalReadCount += readCount;
        }

        if (totalReadCount == count)
        {
            return buffer;
        }

        var result = new byte[totalReadCount];
        Array.Copy(buffer, 0, result, 0, totalReadCount);
        return result;
    }

    private static bool HasMagic(byte[] prefix)
    {
        if (prefix.Length < Magic.Length + 1)
        {
            return false;
        }

        for (var i = 0; i < Magic.Length; i++)
        {
            if (prefix[i] != Magic[i])
            {
                return false;
            }
        }

        return true;
    }

    private static long? TryCalculateEncryptedLength(Stream plainStream, int chunkSize)
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

            var fullChunkCount = plainLength / chunkSize;
            var chunkRecordCount = fullChunkCount + (plainLength % chunkSize > 0 ? 1 : 0) + 1; // +1: terminal record

            checked
            {
                return Magic.Length + 1L + HeaderSize + plainLength +
                       chunkRecordCount * (ChunkLengthPrefixSize + GcmTagSize);
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

#if !NETSTANDARD2_0
    private static AesGcm CreateAesGcm(byte[] keyBytes)
    {
#if NET8_0_OR_GREATER
        return new AesGcm(keyBytes, GcmTagSize);
#else
        return new AesGcm(keyBytes);
#endif
    }
#endif

    private static void WriteInt32BigEndian(byte[] buffer, int offset, int value)
    {
        buffer[offset] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)value;
    }

    private static int ReadInt32BigEndian(byte[] buffer, int offset)
    {
        return (buffer[offset] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | buffer[offset + 3];
    }
}
