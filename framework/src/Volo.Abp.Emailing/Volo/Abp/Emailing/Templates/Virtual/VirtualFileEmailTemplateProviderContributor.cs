using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing.Templates.Virtual
{
    public class VirtualFileEmailTemplateProviderContributor : IEmailTemplateProviderContributor
    {
        public const string VirtualFilePathKey = "VirtualFilePath";

        public Task ProvideAsync(EmailTemplateProviderContributorContext contributorContext)
        {
            var templateDefinition = FindTemplateDefinition(contributorContext);
            if (templateDefinition == null)
            {
                return Task.CompletedTask;
            }

            var fileInfo = FindVirtualFileInfo(contributorContext, templateDefinition);
            if (fileInfo == null)
            {
                return Task.CompletedTask;
            }

            contributorContext.Template = new EmailTemplate(fileInfo.ReadAsString(), templateDefinition);
            return Task.CompletedTask;
        }

        protected virtual EmailTemplateDefinition FindTemplateDefinition(EmailTemplateProviderContributorContext contributorContext)
        {
            return contributorContext
                .ServiceProvider
                .GetRequiredService<IOptions<EmailTemplateOptions>>()
                .Value
                .Templates
                .GetOrDefault(contributorContext.Name);
        }

        protected virtual IFileInfo FindVirtualFileInfo(EmailTemplateProviderContributorContext contributorContext, EmailTemplateDefinition templateDefinition)
        {
            var virtualFilePath = templateDefinition?.GetVirtualFilePathOrNull();
            if (virtualFilePath == null)
            {
                return null;
            }

            var virtualFileProvider = contributorContext.ServiceProvider.GetRequiredService<IVirtualFileProvider>();

            var fileInfo = virtualFileProvider.GetFileInfo(virtualFilePath);
            if (fileInfo?.Exists != true)
            {
                return null;
            }

            return fileInfo;
        }
    }
}