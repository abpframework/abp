using System.Collections.Generic;
using Volo.Abp.Collections;
using Volo.Abp.Emailing.Templates.Virtual;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateOptions
    {
        public List<IEmailTemplateProviderContributor> Providers { get; }

        public string DefaultLayout { get; set; }

        public ITypeList<IEmailTemplateDefinitionProvider> DefinitionProviders { get; }

        public EmailTemplateOptions()
        {
            Providers = new List<IEmailTemplateProviderContributor>
            {
                new VirtualFileEmailTemplateProviderContributor()
            };

            DefaultLayout = StandardEmailTemplates.DefaultLayout;

            DefinitionProviders = new TypeList<IEmailTemplateDefinitionProvider>();
        }
    }
}