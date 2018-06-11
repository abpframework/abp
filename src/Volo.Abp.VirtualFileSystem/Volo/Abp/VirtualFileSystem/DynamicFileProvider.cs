using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.VirtualFileSystem
{
    //TODO: Implement Watch!
    //TODO: Work with dictionaries?
    public class DynamicFileProvider : DictionaryBasedFileProvider, IDynamicFileProvider, ISingletonDependency
    {
        protected override IDictionary<string, IFileInfo> Files => DynamicFiles;

        protected ConcurrentDictionary<string, IFileInfo> DynamicFiles { get; }

        public DynamicFileProvider()
        {
            DynamicFiles = new ConcurrentDictionary<string, IFileInfo>();
        }

        public void AddOrUpdate(IFileInfo fileInfo)
        {
            DynamicFiles.AddOrUpdate(fileInfo.PhysicalPath, fileInfo, (key, value) => fileInfo);
        }

        public bool Delete(string filePath)
        {
            return DynamicFiles.TryRemove(filePath, out _);
        }
    }
}