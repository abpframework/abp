using System;
using System.IO;

namespace Volo.Abp.Storage.LocalStorage
{
    public static class Extensions
    {
        public static StorageException ToStorageException(this Exception ex)
        {
            if(ex is UnauthorizedAccessException)
            {
                throw new StorageException(StorageErrorCode.InvalidAccess.ToStorageError(), ex);
            }
            else if (ex is NotSupportedException 
                || ex is DirectoryNotFoundException 
                || ex is FileNotFoundException
                || ex is ArgumentException)
            {
                throw new StorageException(StorageErrorCode.InvalidName.ToStorageError(), ex);
            }
            else if (ex is IOException)
            {
                throw new StorageException(StorageErrorCode.ErrorOpeningBlob.ToStorageError(), ex);
            }
            else
            {
                return new StorageException(StorageErrorCode.GenericException.ToStorageError(), ex);
            }
        }
    }
}