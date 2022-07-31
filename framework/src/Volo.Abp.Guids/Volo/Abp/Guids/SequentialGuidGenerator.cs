using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Guids;

/* This code is taken from jhtodd/SequentialGuid https://github.com/jhtodd/SequentialGuid/blob/master/SequentialGuid/Classes/SequentialGuid.cs */

/// <summary>
/// Implements <see cref="IGuidGenerator"/> by creating sequential Guids.
/// Use <see cref="AbpSequentialGuidGeneratorOptions"/> to configure.
/// </summary>
public class SequentialGuidGenerator : IGuidGenerator, ITransientDependency
{
    public AbpSequentialGuidGeneratorOptions Options { get; }

    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    public SequentialGuidGenerator(IOptions<AbpSequentialGuidGeneratorOptions> options)
    {
        Options = options.Value;
    }

    public virtual Guid Create()
    {
        return Create(Options.GetDefaultSequentialGuidType());
    }

    public virtual Guid Create(SequentialGuidType guidType)
    {
        // We start with 16 bytes of cryptographically strong random data.
        var randomBytes = new byte[8];
        RandomNumberGenerator.GetBytes(randomBytes);

        // An alternate method: use a normally-created GUID to get our initial
        // random data:
        // byte[] randomBytes = Guid.NewGuid().ToByteArray();
        // This is faster than using RNGCryptoServiceProvider, but I don't
        // recommend it because the .NET Framework makes no guarantee of the
        // randomness of GUID data, and future versions (or different
        // implementations like Mono) might use a different method.

        // Now we have the random basis for our GUID.  Next, we need to
        // create the six-byte block which will be our timestamp.

        // We start with the number of milliseconds that have elapsed since
        // DateTime.MinValue.  This will form the timestamp.  There's no use
        // being more specific than milliseconds, since DateTime.Now has
        // limited resolution.

        // Old:
        // Using millisecond resolution for our 48-bit timestamp gives us
        // about 5900 years before the timestamp overflows and cycles.
        // Hopefully this should be sufficient for most purposes. :)
        // long timestamp = DateTime.UtcNow.Ticks / 10000L;

        // New: 
        // the timestamp generated in the case of high concurrency may be the same, 
        // resulting in the Guid generated at the same time not being sequential.
        // See: https://github.com/abpframework/abp/issues/11453
        long timestamp = DateTime.UtcNow.Ticks;

        // Then get the bytes
        byte[] timestampBytes = BitConverter.GetBytes(timestamp);

        // Since we're converting from an Int64, we have to reverse on
        // little-endian systems.
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        byte[] guidBytes = new byte[16];

        switch (guidType)
        {
            case SequentialGuidType.SequentialAsString:
            case SequentialGuidType.SequentialAsBinary:

                // For string and byte-array version, we copy the timestamp first, followed
                // by the random data.
                Buffer.BlockCopy(timestampBytes, 0, guidBytes, 0, 8);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 8, 8);

                // If formatting as a string, we have to compensate for the fact
                // that .NET regards the Data1 and Data2 block as an Int32 and an Int16,
                // respectively.  That means that it switches the order on little-endian
                // systems.  So again, we have to reverse.
                if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 4);
                    Array.Reverse(guidBytes, 4, 2);
                    Array.Reverse(guidBytes, 6, 2);
                }

                break;

            case SequentialGuidType.SequentialAtEnd:

                // For sequential-at-the-end versions, we copy the random data first,
                // followed by the timestamp.
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 8);

                // MSSQL and System.Data.SqlTypes.SqlGuid sort first by the last 6 Data4 bytes, left to right,
                // then the first two bytes of Data4 (again, left to right),
                // then Data3, Data2, and Data1 right to left.
                Buffer.BlockCopy(timestampBytes, 6, guidBytes, 8, 2);
                Buffer.BlockCopy(timestampBytes, 0, guidBytes, 10, 6);
                break;
        }

        return new Guid(guidBytes);
    }

    /// <summary>
    /// Generate sequential Guids that conform to the RFC 4122 Version 4.
    /// </summary>
    /// <returns></returns>
    public virtual Guid Next()
    {
        return Next(Options.GetDefaultSequentialGuidType());
    }

    /// <summary>
    /// Generate sequential Guids that conform to the RFC 4122 Version 4.
    /// </summary>
    /// <param name="guidType"></param>
    /// <returns></returns>
    public virtual Guid Next(SequentialGuidType guidType)
    {
        // According to RFC 4122 Version 4:
        // dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr
        // - M = RFC version, in this case '4' for random UUID
        // - N = RFC variant (plus other bits), where N is one of 8,9,A, or B, in this case 8 for variant 1
        // - d = nibbles based on UTC date/time in ticks
        // - r = nibbles based on random bytes

        byte version = (byte)4;
        byte variant = (byte)8;
        byte filterHighBit = (byte)0b00001111;
        byte filterLowBit = (byte)0b11110000;

        var randomBytes = new byte[8];
        RandomNumberGenerator.GetBytes(randomBytes);

        long timestamp = DateTime.UtcNow.Ticks;

        byte[] timestampBytes = BitConverter.GetBytes(timestamp);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }

        byte[] guidBytes = new byte[16];

        switch (guidType)
        {
            case SequentialGuidType.SequentialAsString:
            case SequentialGuidType.SequentialAsBinary:

                // AsString: dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr

                // dddddddd-dddd
                Buffer.BlockCopy(timestampBytes, 0, guidBytes, 0, 6);
                // Md
                guidBytes[6] = (byte)((version << 4) | ((timestampBytes[6] & filterLowBit) >> 4));
                // dd
                guidBytes[7] = (byte)(((timestampBytes[6] & filterHighBit) << 4) | ((timestampBytes[7] & filterLowBit) >> 4));
                // Nd
                guidBytes[8] = (byte)((variant << 4) | (timestampBytes[7] & filterHighBit));
                // rr-rrrrrrrrrrrr
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 9, 7);

                // If formatting as a string, we have to compensate for the fact
                // that .NET regards the Data1, Data2 and Data3 block as an Int32, an Int16 and an Int16,
                // respectively.  That means that it switches the order on little-endian
                // systems.  So again, we have to reverse.
                if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 4);
                    Array.Reverse(guidBytes, 4, 2);
                    Array.Reverse(guidBytes, 6, 2);
                }

                break;

            case SequentialGuidType.SequentialAtEnd:

                // MSSQL and System.Data.SqlTypes.SqlGuid sort first by the last 6 Data4 bytes, left to right,
                // then the first two bytes of Data4 (again, left to right),
                // then Data3, Data2, and Data1 right to left.

                // AtEnd: rrrrrrrr-rrrr-Mxdr-Nddd-dddddddddddd
                // Block: 1        2    3    4    5
                // Order：Block5 >> Block4 >> Block3 >> Block2 >> Block1

                // rrrrrrrr-rrrr
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 6);
                // Mx
                guidBytes[6] = (byte)(version << 4);
                // dr
                guidBytes[7] = (byte)(((timestampBytes[7] & filterHighBit) << 4) | (randomBytes[7] & filterHighBit));
                // Nd
                guidBytes[8] = (byte)((variant << 4) | ((timestampBytes[6] & filterLowBit) >> 4));
                // dd
                guidBytes[9] = (byte)(((timestampBytes[6] & filterHighBit) << 4) | ((timestampBytes[7] & filterLowBit) >> 4));
                // dddddddddddd
                Buffer.BlockCopy(timestampBytes, 0, guidBytes, 10, 6);

                // .NET regards the Data3 block as an Int16, we have to reverse it.
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 6, 2);
                }
                break;
        }

        return new Guid(guidBytes);
    }
}
