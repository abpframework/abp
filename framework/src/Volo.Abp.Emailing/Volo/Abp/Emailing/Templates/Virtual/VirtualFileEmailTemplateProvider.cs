using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing.Templates.Virtual
{
    public class VirtualFileEmailTemplateProvider : IEmailTemplateProvider
    {
        public const string VirtualFilePathKey = "VirtualFilePath";

        public Task ProvideAsync(EmailTemplateProviderContext context)
        {
            var templateDefinition = FindTemplateDefinition(context);
            if (templateDefinition == null)
            {
                return Task.CompletedTask;
            }

            var fileInfo = FindVirtualFileInfo(context, templateDefinition);
            if (fileInfo == null)
            {
                return Task.CompletedTask;
            }

            context.Template = new EmailTemplate(fileInfo.ReadAsString());
            return Task.CompletedTask;
        }

        protected virtual EmailTemplateDefinition FindTemplateDefinition(EmailTemplateProviderContext context)
        {
            return context
                .ServiceProvider
                .GetRequiredService<IOptions<EmailTemplateOptions>>()
                .Value
                .Templates
                .GetOrDefault(context.Name);
        }

        protected virtual IFileInfo FindVirtualFileInfo(EmailTemplateProviderContext context, EmailTemplateDefinition templateDefinition)
        {
            var virtualFilePath = templateDefinition?.GetVirtualFilePathOrNull();
            if (virtualFilePath == null)
            {
                return null;
            }

            var virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();

            var fileInfo = virtualFileProvider.GetFileInfo(virtualFilePath);
            if (fileInfo?.Exists != true)
            {
                return null;
            }

            return fileInfo;
        }
    }
}