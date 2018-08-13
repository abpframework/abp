using System;

namespace Volo.Abp.Storage.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        public FileAlreadyExistsException(string storeName, string filePath)
            : base($"The file {filePath} already exists in Store {storeName}.")
        {
        }
    }
}