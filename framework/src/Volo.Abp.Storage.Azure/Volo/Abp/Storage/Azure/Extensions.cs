using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.WindowsAzure.Storage.Blob;
using AzureStorageException = Microsoft.WindowsAzure.Storage.StorageException;

namespace Volo.Abp.Storage.Azure
{
    public static class Extensions
    {
        public static SharedAccessBlobPermissions ToPermissions(this BlobUrlAccess security)
        {
            switch (security)
            {
                case BlobUrlAccess.Read:
                    return SharedAccessBlobPermissions.Read;
                case BlobUrlAccess.Write:
                    return SharedAccessBlobPermissions.Create | SharedAccessBlobPermissions.Write;
                case BlobUrlAccess.Delete:
                    return SharedAccessBlobPermissions.Delete;
                case BlobUrlAccess.All:
                    return SharedAccessBlobPermissions.Create | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;
                default:
                    return SharedAccessBlobPermissions.None;
            }
        }

        public static Exception Convert(this Exception e)
        {
            var storageException = (e as AzureStorageException) ?? e.InnerException as AzureStorageException;

            if (storageException != null)
            {
                StorageErrorCode errorCode;

                switch ((HttpStatusCode)storageException.RequestInformation.HttpStatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        errorCode = StorageErrorCode.InvalidCredentials;
                        break;
                    case HttpStatusCode.NotFound:
                        errorCode = StorageErrorCode.InvalidName;
                        break;
                    default:
                        errorCode = StorageErrorCode.GenericException;
                        break;
                }

                return new StorageException(errorCode.ToStorageError(), storageException);
            }
            return e;
        }

        public static bool IsAzureStorageException(this Exception e)
        {
            return e is AzureStorageException || e.InnerException is AzureStorageException;
        }

        public static void SetMetadata(this IDictionary<string, string> azureMeta, IDictionary<string, string> meta)
        {
            azureMeta.Clear();

            if (meta != null)
            {
                foreach (var kvp in meta)
                {
                    azureMeta[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}