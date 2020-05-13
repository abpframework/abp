using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class LocalizedTemplateContentReaderFactory : ILocalizedTemplateContentReaderFactory, ISingletonDependency
    {
        private readonly IVirtualFileProvider _virtualFileProvider;
        private readonly ConcurrentDictionary<string, ILocalizedTemplateContentReader> _readerCache;
        protected SemaphoreSlim SyncObj;

        public LocalizedTemplateContentReaderFactory(IVirtualFileProvider virtualFileProvider)
        {
            _virtualFileProvider = virtualFileProvider;
            _readerCache = new ConcurrentDictionary<string, ILocalizedTemplateContentReader>();
            SyncObj = new SemaphoreSlim(1, 1);
        }

        public async Task<ILocalizedTemplateContentReader> CreateAsync(TemplateDefinition templateDefinition)
        {
            if (_readerCache.TryGetValue(templateDefinition.Name, out var reader))
            {
                return reader;
            }

            using (await SyncObj.LockAsync())
            {
                if (_readerCache.TryGetValue(templateDefinition.Name, out reader))
                {
                    return reader;
                }

                reader = await CreateInternalAsync(templateDefinition);
                _readerCache[templateDefinition.Name] = reader;
                return reader;
            }
        }

        protected virtual async Task<ILocalizedTemplateContentReader> CreateInternalAsync(
            TemplateDefinition templateDefinition)
        {
            var virtualPath = templateDefinition.GetVirtualFilePathOrNull();
            if (virtualPath == null)
            {
                return NullLocalizedTemplateContentReader.Instance;
            }

            var fileInfo = _virtualFileProvider.GetFileInfo(virtualPath);
            if (!fileInfo.Exists)
            {
                throw new AbpException("Could not find a file/folder at the location: " + virtualPath);
            }

            if (fileInfo.IsDirectory)
            {
                var folderReader = new VirtualFolderLocalizedTemplateContentReader();
                await folderReader.ReadContentsAsync(_virtualFileProvider, virtualPath);
                return folderReader;
            }
            else //File
            {
                var singleFileReader = new FileInfoLocalizedTemplateContentReader();
                await singleFileReader.ReadContentsAsync(fileInfo);
                return singleFileReader;
            }
        }
    }
}