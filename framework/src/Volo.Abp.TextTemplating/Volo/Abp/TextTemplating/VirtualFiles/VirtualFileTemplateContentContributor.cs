using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class VirtualFileTemplateContentContributor : ITemplateContentContributor, ITransientDependency
    {
        public const string VirtualPathPropertyName = "VirtualPath";

        private readonly IVirtualFileProvider _virtualFileProvider;

        public VirtualFileTemplateContentContributor(IVirtualFileProvider virtualFileProvider)
        {
            _virtualFileProvider = virtualFileProvider;
        }

        public virtual async Task<string> GetOrNullAsync(TemplateContentContributorContext context)
        {
            var cultureName = context.Culture ??
                              CultureInfo.CurrentUICulture.Name;

            var localizedReader = await CreateLocalizedReader(context);

            return localizedReader.GetContent(
                cultureName,
                context.TemplateDefinition.DefaultCultureName
            );
        }

        protected async Task<ILocalizedTemplateContentReader> CreateLocalizedReader(
            TemplateContentContributorContext context)
        {
            var virtualPath = context
                .TemplateDefinition
                .Properties
                .GetOrDefault(VirtualPathPropertyName) as string;

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