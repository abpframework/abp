using System.Collections.Generic;
using Volo.Abp.Emailing.Templates.Virtual;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateOptions
    {
        public List<IEmailTemplateProvider> Providers { get; }

        public EmailTemplateDefinitionDictionary Templates { get; }

        public EmailTemplateOptions()
        {
            Providers = new List<IEmailTemplateProvider>
            {
                new VirtualFileEmailTemplateProvider()
            };

            Templates = new EmailTemplateDefinitionDictionary();
        }
    }
}