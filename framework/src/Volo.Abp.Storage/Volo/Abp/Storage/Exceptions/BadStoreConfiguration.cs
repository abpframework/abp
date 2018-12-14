using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Exceptions
{
    public class BadStoreConfiguration : Exception
    {
        public BadStoreConfiguration(string storeName)
            : base($"The store '{storeName}' was not properly configured.")
        {
        }

        public BadStoreConfiguration(string storeName, string details)
            : base($"The store '{storeName}' was not properly configured. {details}")
        {
        }

        public BadStoreConfiguration(string storeName, IEnumerable<IAbpStorageOptionError> errors)
            : this(storeName, string.Join(" | ", errors.Select(e => e.ErrorMessage)))
        {
            Errors = errors;
        }

        public IEnumerable<IAbpStorageOptionError> Errors { get; }
    }
}