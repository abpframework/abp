using Microsoft.Extensions.Logging;
using Volo.Abp.Logging;

namespace Volo.Abp.Storage
{
    public class FileAlreadyExistsException : AbpException, IHasLogLevel
    {
        public FileAlreadyExistsException(string storeName, string filePath)
            : base($"The file {filePath} already exists in Store {storeName}.")
        {
            LogLevel = LogLevel.Error;
        }

        public LogLevel LogLevel { get; set; }
    }
}