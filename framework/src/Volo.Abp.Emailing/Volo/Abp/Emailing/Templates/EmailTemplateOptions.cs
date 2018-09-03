using System.Collections.Generic;
using Volo.Abp.Emailing.Templates.Virtual;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateOptions
    {
        public List<IEmailTemplateProviderContributor> Providers { get; }

        public EmailTemplateDefinitionDictionary Templates { get; }

        public string DefaultLayout { get; set; }

        public EmailTemplateOptions()
        {
            Providers = new List<IEmailTemplateProviderContributor>
            {
                new VirtualFileEmailTemplateProviderContributor()
            };

            Templates = new EmailTemplateDefinitionDictionary();

            DefaultLayout = StandardEmailTemplates.DefaultLayout;
        }
    }
}