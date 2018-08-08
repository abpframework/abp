using System;

namespace Volo.Abp.Storage
{
    public class StorageException : Exception
    {
        public int ErrorCode { get; private set; }

        public string ProviderMessage { get; set; }

        public StorageException(StorageError error, Exception ex) 
            : base(error.Message, ex)
        {
            ErrorCode = error.Code;
            ProviderMessage = ex?.Message;
        }

        public StorageException(StorageErrorCode errorCode, Exception ex)
            : base(errorCode.ToStorageError().Message, ex)
        {
            ErrorCode = (int)errorCode;
            ProviderMessage = ex?.Message;
        }

        public StorageException(StorageErrorCode errorCode)
            : this(errorCode, (Exception)null) { }

        public StorageException(StorageErrorCode errorCode, string message)
            : base(message) 
        { 
            ErrorCode = (int)errorCode;
        }
    }
}