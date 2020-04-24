using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class LocalizedTemplateContentReaderFactory : ILocalizedTemplateContentReaderFactory, ISingletonDependency
    {
        private readonly IVirtualFileProvider _virtualFileProvider;

        public LocalizedTemplateContentReaderFactory(IVirtualFileProvider virtualFileProvider)
        {
            _virtualFileProvider = virtualFileProvider;
        }

        public Task<ILocalizedTemplateContentReader> CreateAsync(TemplateDefinition templateDefinition)
        {
            return CreateLocalizedReader(templateDefinition);
        }

        protected async Task<ILocalizedTemplateContentReader> CreateLocalizedReader(
            TemplateDefinition templateDefinition)
        {
            var virtualPath = templateDefinition
                .Properties
                .GetOrDefault(VirtualFileTemplateContentContributor.VirtualPathPropertyName) as string;

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
                var folderReader = new FolderLocalizedTemplateContentReader();
                await folderReader.ReadContentsAsync(_virtualFileProvider, virtualPath);
                return folderReader;
            }
            else //File
            {
                var singleFileReader = new SingleFileLocalizedTemplateContentReader();
                await singleFileReader.ReadContentsAsync(fileInfo);
                return singleFileReader;
            }
        }
    }
}