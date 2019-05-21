using Volo.Abp.Collections;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateOptions
    {
        public string DefaultLayout { get; set; }

        public ITypeList<IEmailTemplateDefinitionProvider> DefinitionProviders { get; }

        public EmailTemplateOptions()
        {
            DefaultLayout = StandardEmailTemplates.DefaultLayout;

            DefinitionProviders = new TypeList<IEmailTemplateDefinitionProvider>();
        }
    }
}