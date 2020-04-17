using Volo.Abp.Collections;

namespace Volo.Abp.Emailing.Templates
{
    public class AbpEmailTemplateOptions
    {
        public string DefaultLayout { get; set; }

        public ITypeList<IEmailTemplateDefinitionProvider> DefinitionProviders { get; }

        public AbpEmailTemplateOptions()
        {
            DefaultLayout = StandardEmailTemplates.DefaultLayout;

            DefinitionProviders = new TypeList<IEmailTemplateDefinitionProvider>();
        }
    }
}