using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Security.Encryption;

/// <summary>
/// Implements <see cref="IByteArrayEncryptionService"/> using authenticated encryption.
/// Uses AES-256-GCM on platforms that support it, and falls back to
/// AES-256-CBC + HMAC-SHA256 (encrypt-then-MAC) on .NET Standard 2.0.
/// <para>
/// Output format: a 14-byte header (version, algorithm, chunk size, base nonce),
/// followed by authenticated chunks: 4-byte big-endian cipher length, cipher chunk, authentication tag,
/// and an authenticated zero-length terminal record.
/// The header and the chunk index are bound to every chunk as associated data,
/// so chunks can not be re-ordered, truncated or moved between files.
/// </para>
/// </summary>
public class ByteArrayEncryptionService : IByteArrayEncryptionService, ITransientDependency
{
    protected AbpByteArrayEncryptionOptions Options { get; }

    protected const byte FormatVersion = 1;
    protected const byte AlgorithmAesGcm = 1;
    protected const byte AlgorithmAesCbcHmacSha256 = 2;
    protected const int BaseNonceSize = 8;
    protected const int HeaderSize = 14; // version(1) + algorithm(1) + chunkSize(4) + baseNonce(8)
    protected const int ChunkLengthPrefixSize = 4;
    protected const int GcmNonceSize = 12;
    protected const int GcmTagSize = 16;
    protected const int HmacSize = 32;
    protected const int AesBlockSize = 16;
    protected const int MaximumChunkSize = 16 * 1024 * 1024;

    public ByteArrayEncryptionService(IOptions<AbpByteArrayEncryptionOptions> options)
    {
        Options = options.Value;
    }

    public virtual byte[]? Encrypt(byte[]? plainBytes, string? passPhrase = null, byte[]? salt = null)
    {
        if (plainBytes == null)
        {
            return null;
        }

        using var plainStream = new MemoryStream(plainBytes, writable: false);
        using var cipherStream = new MemoryStream();
        Encrypt(plainStream, cipherStream, passPhrase, salt);
        return cipherStream.ToArray();
    }

    public virtual byte[]? Decrypt(byte[]? cipherBytes, string? passPhrase = null, byte[]? salt = null)
    {
        if (cipherBytes == null || cipherBytes.Length == 0)
        {
            return null;
        }

        using var cipherStream = new MemoryStream(cipherBytes, writable: false);
        using var plainStream = new MemoryStream();
        Decrypt(cipherStream, plainStream, passPhrase, salt);
        return plainStream.ToArray();
    }

    public virtual void Encrypt(Stream plainStream, Stream cipherStream, string? passPhrase = null, byte[]? salt = null)
    {
        Check.NotNull(plainStream, nameof(plainStream));
        Check.NotNull(cipherStream, nameof(cipherStream));

        if (Options.ChunkSize <= 0 || Options.ChunkSize > MaximumChunkSize)
        {
            throw new AbpException($"{nameof(Options.ChunkSize)} must be between 1 and {MaximumChunkSize} bytes!");
        }

        var algorithm = GetEncryptionAlgorithm();
        var keyBytes = DeriveKeyBytes(passPhrase ?? Options.DefaultPassPhrase, salt ?? Options.DefaultSalt, algorithm);

        var baseNonce = new byte[BaseNonceSize];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(baseNonce);
        }

        var header = BuildHeader(algorithm, Options.ChunkSize, baseNonce);
        cipherStream.Write(header, 0, header.Length);

        var buffer = new byte[Options.ChunkSize];
        var chunkIndex = 0;
        int readCount;
        while ((readCount = plainStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            EncryptChunk(algorithm, keyBytes, header, baseNonce, chunkIndex, buffer, readCount, cipherStream);
            chunkIndex++;
        }

        WriteTerminalRecord(algorithm, keyBytes, header, baseNonce, chunkIndex, cipherStream);
    }

    public virtual void Decrypt(Stream cipherStream, Stream plainStream, string? passPhrase = null, byte[]? salt = null)
    {
        Check.NotNull(cipherStream, nameof(cipherStream));
        Check.NotNull(plainStream, nameof(plainStream));

        var header = ReadExactly(cipherStream, HeaderSize);
        if (header == null)
        {
            throw new AbpException("The encrypted data is corrupted or has an invalid format: missing header!");
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
            throw new AbpException("The encrypted data is corrupted or has an invalid format: invalid chunk size!");
        }

        var baseNonce = new byte[BaseNonceSize];
        Array.Copy(header, 6, baseNonce, 0, BaseNonceSize);

        var keyBytes = DeriveKeyBytes(passPhrase ?? Options.DefaultPassPhrase, salt ?? Options.DefaultSalt, algorithm);

        var tagSize = algorithm == AlgorithmAesGcm ? GcmTagSize : HmacSize;
        var maxCipherChunkSize = algorithm == AlgorithmAesGcm ? chunkSize : chunkSize + AesBlockSize;

        var chunkIndex = 0;
        while (true)
        {
            var lengthPrefix = ReadUpTo(cipherStream, ChunkLengthPrefixSize);
            if (lengthPrefix.Length == 0)
            {
                throw new AbpException("The encrypted data is corrupted or has an invalid format: missing terminal record!");
            }

            if (lengthPrefix.Length < ChunkLengthPrefixSize)
            {
                throw new AbpException("The encrypted data is corrupted or has an invalid format: truncated chunk!");
            }

            var cipherChunkSize = ReadInt32BigEndian(lengthPrefix, 0);
            if (cipherChunkSize == 0)
            {
                var terminalTag = ReadExactly(cipherStream, tagSize);
                if (terminalTag == null || ReadUpTo(cipherStream, 1).Length != 0)
                {
                    throw new AbpException("The encrypted data is corrupted or has an invalid format: invalid terminal record!");
                }

                VerifyTerminalRecord(algorithm, keyBytes, header, baseNonce, chunkIndex, terminalTag);
                break;
            }

            if (cipherChunkSize < 0 || cipherChunkSize > maxCipherChunkSize)
            {
                throw new AbpException("The encrypted data is corrupted or has an invalid format: invalid chunk length!");
            }

            var cipherChunk = ReadExactly(cipherStream, cipherChunkSize);
            var tag = ReadExactly(cipherStream, tagSize);
            if (cipherChunk == null || tag == null)
            {
                throw new AbpException("The encrypted data is corrupted or has an invalid format: truncated chunk!");
            }

            var plainChunk = DecryptChunk(algorithm, keyBytes, header, baseNonce, chunkIndex, cipherChunk, tag);
            plainStream.Write(plainChunk, 0, plainChunk.Length);
            chunkIndex++;
        }
    }

    /// <summary>
    /// Gets the algorithm used while encrypting. Decryption supports both algorithms
    /// (except AES-GCM on .NET Standard 2.0, where it is not available).
    /// </summary>
    protected virtual byte GetEncryptionAlgorithm()
    {
#if NETSTANDARD2_0
        return AlgorithmAesCbcHmacSha256;
#else
        return AlgorithmAesGcm;
#endif
    }

    /// <summary>
    /// Derives the key material using PBKDF2-SHA1 (Rfc2898DeriveBytes). SHA1 is used on all
    /// target frameworks on purpose, so that the derived key is deterministic across platforms.
    /// Returns 32 bytes for AES-256-GCM, or 64 bytes (32 encryption + 32 MAC) for AES-256-CBC-HMAC.
    /// </summary>
    protected virtual byte[] DeriveKeyBytes(string passPhrase, byte[] salt, byte algorithm)
    {
        var keyLength = algorithm == AlgorithmAesGcm ? 32 : 64;
#if NET8_0_OR_GREATER
        return Rfc2898DeriveBytes.Pbkdf2(passPhrase, salt, Options.DeriveBytesIterations, HashAlgorithmName.SHA1, keyLength);
#else
        // The default hash algorithm of this constructor is SHA1.
        using var password = new Rfc2898DeriveBytes(passPhrase, salt, Options.DeriveBytesIterations);
        return password.GetBytes(keyLength);
#endif
    }

    protected virtual void EncryptChunk(byte algorithm, byte[] keyBytes, byte[] header, byte[] baseNonce, int chunkIndex, byte[] plainChunk, int plainChunkLength, Stream cipherStream)
    {
        var associatedData = CreateChunkAssociatedData(header, chunkIndex);
        byte[] cipherChunk;
        byte[] tag;

        if (algorithm == AlgorithmAesGcm)
        {
#if NETSTANDARD2_0
            throw new AbpException("AES-GCM is not supported on this platform (.NET Standard 2.0)!");
#else
            cipherChunk = new byte[plainChunkLength];
            tag = new byte[GcmTagSize];
            using (var aesGcm = CreateAesGcm(keyBytes))
            {
                aesGcm.Encrypt(CreateChunkNonce(baseNonce, chunkIndex), plainChunk.AsSpan(0, plainChunkLength), cipherChunk, tag, associatedData);
            }
#endif
        }
        else
        {
            cipherChunk = AesCbcEncryptChunk(keyBytes, baseNonce, chunkIndex, plainChunk, plainChunkLength);
            tag = ComputeChunkMac(keyBytes, associatedData, cipherChunk);
        }

        var lengthPrefix = new byte[ChunkLengthPrefixSize];
        WriteInt32BigEndian(lengthPrefix, 0, cipherChunk.Length);
        cipherStream.Write(lengthPrefix, 0, lengthPrefix.Length);
        cipherStream.Write(cipherChunk, 0, cipherChunk.Length);
        cipherStream.Write(tag, 0, tag.Length);
    }

    protected virtual void WriteTerminalRecord(
        byte algorithm,
        byte[] keyBytes,
        byte[] header,
        byte[] baseNonce,
        int chunkIndex,
        Stream cipherStream)
    {
        var lengthPrefix = new byte[ChunkLengthPrefixSize];
        cipherStream.Write(lengthPrefix, 0, lengthPrefix.Length);

        var tag = ComputeTerminalTag(algorithm, keyBytes, header, baseNonce, chunkIndex);
        cipherStream.Write(tag, 0, tag.Length);
    }

    protected virtual void VerifyTerminalRecord(
        byte algorithm,
        byte[] keyBytes,
        byte[] header,
        byte[] baseNonce,
        int chunkIndex,
        byte[] tag)
    {
        if (algorithm == AlgorithmAesGcm)
        {
#if NETSTANDARD2_0
            throw new AbpException("AES-GCM encrypted data can not be decrypted on this platform (.NET Standard 2.0)!");
#else
            using (var aesGcm = CreateAesGcm(keyBytes))
            {
                aesGcm.Decrypt(
                    CreateChunkNonce(baseNonce, chunkIndex),
                    Array.Empty<byte>(),
                    tag,
                    Array.Empty<byte>(),
                    CreateChunkAssociatedData(header, chunkIndex)
                );
            }
#endif
        }
        else
        {
            var expectedTag = ComputeTerminalTag(algorithm, keyBytes, header, baseNonce, chunkIndex);
            if (!FixedTimeEquals(expectedTag, tag))
            {
                throw new CryptographicException("The encrypted data is tampered, corrupted or the passphrase/salt is wrong!");
            }
        }
    }

    protected virtual byte[] ComputeTerminalTag(
        byte algorithm,
        byte[] keyBytes,
        byte[] header,
        byte[] baseNonce,
        int chunkIndex)
    {
        var associatedData = CreateChunkAssociatedData(header, chunkIndex);
        if (algorithm == AlgorithmAesGcm)
        {
#if NETSTANDARD2_0
            throw new AbpException("AES-GCM is not supported on this platform (.NET Standard 2.0)!");
#else
            var tag = new byte[GcmTagSize];
            using (var aesGcm = CreateAesGcm(keyBytes))
            {
                aesGcm.Encrypt(
                    CreateChunkNonce(baseNonce, chunkIndex),
                    Array.Empty<byte>(),
                    Array.Empty<byte>(),
                    tag,
                    associatedData
                );
            }

            return tag;
#endif
        }

        return ComputeChunkMac(keyBytes, associatedData, Array.Empty<byte>());
    }

    protected virtual byte[] DecryptChunk(byte algorithm, byte[] keyBytes, byte[] header, byte[] baseNonce, int chunkIndex, byte[] cipherChunk, byte[] tag)
    {
        var associatedData = CreateChunkAssociatedData(header, chunkIndex);

        if (algorithm == AlgorithmAesGcm)
        {
#if NETSTANDARD2_0
            throw new AbpException("AES-GCM encrypted data can not be decrypted on this platform (.NET Standard 2.0)!");
#else
            var plainChunk = new byte[cipherChunk.Length];
            using (var aesGcm = CreateAesGcm(keyBytes))
            {
                // Throws CryptographicException if the authentication tag is invalid.
                aesGcm.Decrypt(CreateChunkNonce(baseNonce, chunkIndex), cipherChunk, tag, plainChunk, associatedData);
            }

            return plainChunk;
#endif
        }
        else
        {
            var expectedTag = ComputeChunkMac(keyBytes, associatedData, cipherChunk);
            if (!FixedTimeEquals(expectedTag, tag))
            {
                throw new CryptographicException("The encrypted data is tampered, corrupted or the passphrase/salt is wrong!");
            }

            return AesCbcDecryptChunk(keyBytes, baseNonce, chunkIndex, cipherChunk);
        }
    }

    /// <summary>
    /// Creates the 12-byte nonce of a chunk: 8-byte random base nonce + 4-byte big-endian chunk index.
    /// Since the base nonce is random per encryption operation and the index is unique per chunk,
    /// a nonce never repeats for the same key.
    /// </summary>
    protected virtual byte[] CreateChunkNonce(byte[] baseNonce, int chunkIndex)
    {
        var nonce = new byte[GcmNonceSize];
        Array.Copy(baseNonce, 0, nonce, 0, BaseNonceSize);
        WriteInt32BigEndian(nonce, BaseNonceSize, chunkIndex);
        return nonce;
    }

    /// <summary>
    /// Creates the associated data of a chunk: the header + 4-byte big-endian chunk index.
    /// This binds every chunk to its position and to the file it belongs to.
    /// </summary>
    protected virtual byte[] CreateChunkAssociatedData(byte[] header, int chunkIndex)
    {
        var associatedData = new byte[HeaderSize + 4];
        Array.Copy(header, 0, associatedData, 0, HeaderSize);
        WriteInt32BigEndian(associatedData, HeaderSize, chunkIndex);
        return associatedData;
    }

    /// <summary>
    /// Encrypts a chunk with AES-256-CBC. The IV of each chunk is derived from the MAC key,
    /// the base nonce and the chunk index, so it is unique and unpredictable per chunk.
    /// </summary>
    protected virtual byte[] AesCbcEncryptChunk(byte[] keyBytes, byte[] baseNonce, int chunkIndex, byte[] plainChunk, int plainChunkLength)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        using var encryptor = aes.CreateEncryptor(GetAesCbcEncryptionKey(keyBytes), DeriveAesCbcChunkIV(keyBytes, baseNonce, chunkIndex));
        return encryptor.TransformFinalBlock(plainChunk, 0, plainChunkLength);
    }

    protected virtual byte[] AesCbcDecryptChunk(byte[] keyBytes, byte[] baseNonce, int chunkIndex, byte[] cipherChunk)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        using var decryptor = aes.CreateDecryptor(GetAesCbcEncryptionKey(keyBytes), DeriveAesCbcChunkIV(keyBytes, baseNonce, chunkIndex));
        return decryptor.TransformFinalBlock(cipherChunk, 0, cipherChunk.Length);
    }

    /// <summary>
    /// Computes the HMAC-SHA256 of a chunk over its associated data and cipher bytes (encrypt-then-MAC).
    /// </summary>
    protected virtual byte[] ComputeChunkMac(byte[] keyBytes, byte[] associatedData, byte[] cipherChunk)
    {
        using var hmac = new HMACSHA256(GetAesCbcMacKey(keyBytes));
        hmac.TransformBlock(associatedData, 0, associatedData.Length, null, 0);
        hmac.TransformFinalBlock(cipherChunk, 0, cipherChunk.Length);
        return hmac.Hash!;
    }

    /// <summary>
    /// Derives the IV of a CBC chunk: first 16 bytes of HMAC-SHA256(MAC key, "IV" + chunk nonce).
    /// </summary>
    protected virtual byte[] DeriveAesCbcChunkIV(byte[] keyBytes, byte[] baseNonce, int chunkIndex)
    {
        var input = CreateChunkNonce(baseNonce, chunkIndex);
        input[0] ^= 0xFF; // Domain separation from the GCM nonce, just in case.
        using var hmac = new HMACSHA256(GetAesCbcMacKey(keyBytes));
        var hash = hmac.ComputeHash(input);
        var iv = new byte[AesBlockSize];
        Array.Copy(hash, 0, iv, 0, AesBlockSize);
        return iv;
    }

    protected virtual byte[] GetAesCbcEncryptionKey(byte[] keyBytes)
    {
        var key = new byte[32];
        Array.Copy(keyBytes, 0, key, 0, 32);
        return key;
    }

    protected virtual byte[] GetAesCbcMacKey(byte[] keyBytes)
    {
        var key = new byte[32];
        Array.Copy(keyBytes, 32, key, 0, 32);
        return key;
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

    protected virtual byte[] BuildHeader(byte algorithm, int chunkSize, byte[] baseNonce)
    {
        var header = new byte[HeaderSize];
        header[0] = FormatVersion;
        header[1] = algorithm;
        WriteInt32BigEndian(header, 2, chunkSize);
        Array.Copy(baseNonce, 0, header, 6, BaseNonceSize);
        return header;
    }

    /// <summary>
    /// Reads exactly <paramref name="count"/> bytes from the stream.
    /// Returns null if the stream ends before <paramref name="count"/> bytes could be read.
    /// </summary>
    protected virtual byte[]? ReadExactly(Stream stream, int count)
    {
        var buffer = ReadUpTo(stream, count);
        return buffer.Length == count ? buffer : null;
    }

    /// <summary>
    /// Reads up to <paramref name="count"/> bytes from the stream.
    /// May return fewer bytes (or an empty array) only if the stream ends.
    /// </summary>
    protected virtual byte[] ReadUpTo(Stream stream, int count)
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

    private static bool FixedTimeEquals(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        var diff = 0;
        for (var i = 0; i < a.Length; i++)
        {
            diff |= a[i] ^ b[i];
        }

        return diff == 0;
    }
}
