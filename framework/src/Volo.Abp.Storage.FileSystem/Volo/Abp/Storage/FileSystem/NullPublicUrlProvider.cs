using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Storage.FileSystem
{
    public class NullPublicUrlProvider : IAbpPublicUrlProvider, ITransientDependency
    {
        public string GetPublicUrl(string storeName, FileSystemFileReference file)
        {
            throw new InvalidOperationException("There is no public url provider!");
        }
    }
}
