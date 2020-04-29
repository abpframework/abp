using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class LocalizedTemplateContentReaderFactory : ILocalizedTemplateContentReaderFactory, ISingletonDependency
    {
        private readonly IVirtualFileProvider _virtualFileProvider;
        private readonly Dictionary<string, ILocalizedTemplateContentReader> _readerCache;
        private readonly ReaderWriterLockSlim _lock;

        public LocalizedTemplateContentReaderFactory(IVirtualFileProvider virtualFileProvider)
        {
            _virtualFileProvider = virtualFileProvider;
            _readerCache = new Dictionary<string, ILocalizedTemplateContentReader>();
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        public async Task<ILocalizedTemplateContentReader> CreateAsync(TemplateDefinition templateDefinition)
        {
            _lock.EnterUpgradeableReadLock();

            try
            {
                var reader = _readerCache.GetOrDefault(templateDefinition.Name);
                if (reader != null)
                {
                    return reader;
                }

                _lock.EnterWriteLock();

                try
                {
                    reader = await CreateInternalAsync(templateDefinition);
                    _readerCache[templateDefinition.Name] = reader;
                    return reader;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
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