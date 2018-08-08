using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Storage
{
    public static class Extentions
    {
        public static long ToUnixTimeSeconds(this DateTimeOffset dateTimeOffset)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var unixTimeStampInTicks = (dateTimeOffset.ToUniversalTime() - unixStart).Ticks;
            return unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }

        public static StorageError ToStorageError(this int code)
            => ((StorageErrorCode) code).ToStorageError();

        public static StorageError ToStorageError(this StorageErrorCode code)
        {
            var error = Errors
                .Where(x => x.Key == code)
                .Select(x => new StorageError() {Code = (int) x.Key, Message = x.Value})
                .FirstOrDefault();

            return error ?? new StorageError
                   {
                       Code = (int) StorageErrorCode.GenericException,
                       Message = "Generic provider exception occurred",
                   };
        }

        public static List<T2> SelectToListOrEmpty<T1, T2>(this IEnumerable<T1> e, Func<T1, T2> f)
            => e == null ? new List<T2>() : e.Select(f).ToList();

        public static List<T1> WhereToListOrEmpty<T1>(this IEnumerable<T1> e, Func<T1, bool> f)
            => e == null ? new List<T1>() : e.Where(f).ToList();

        private static readonly Dictionary<StorageErrorCode, string> Errors = new Dictionary<StorageErrorCode, string>
        {
            {
                StorageErrorCode.InvalidCredentials,
                "Invalid security credentials"
            },
            {
                StorageErrorCode.GenericException,
                "Generic provider exception occurred"
            },
            {
                StorageErrorCode.InvalidAccess,
                "Invalid access permissions."
            },
            {
                StorageErrorCode.BlobInUse,
                "Blob in use."
            },
            {
                StorageErrorCode.InvalidName,
                "Invalid blob or container name."
            },
            {
                StorageErrorCode.ErrorOpeningBlob,
                "Error opening blob."
            },
            {
                StorageErrorCode.NoCredentialsProvided,
                "No credentials provided."
            },
            {
                StorageErrorCode.NotFound,
                "Blob or container not found."
            }
        };
    }
}