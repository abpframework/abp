using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing.Templates.VirtualFiles
{
    public class SingleVirtualFileEmailTemplateContributor : IEmailTemplateContributor
    {
        private readonly string _virtualPath;

        private IVirtualFileProvider _virtualFileProvider;

        public SingleVirtualFileEmailTemplateContributor(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public void Initialize(EmailTemplateInitializationContext context)
        {
            _virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();
        }

        public string GetOrNull(string cultureName)
        {
            var file = _virtualFileProvider.GetFileInfo(_virtualPath);
            if (file == null || !file.Exists || file.IsDirectory)
            {
                return null;
            }

            return file.ReadAsString();
        }
    }
}