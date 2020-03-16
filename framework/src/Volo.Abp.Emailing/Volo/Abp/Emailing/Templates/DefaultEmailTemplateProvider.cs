using Volo.Abp.Emailing.Templates.VirtualFiles;

namespace Volo.Abp.Emailing.Templates
{
    public class DefaultEmailTemplateProvider : EmailTemplateDefinitionProvider
    {
        public override void Define(IEmailTemplateDefinitionContext context)
        {
            context.Add(new EmailTemplateDefinition(StandardEmailTemplates.DefaultLayout, defaultCultureName: "en", isLayout: true, layout: null)
                .AddTemplateVirtualFiles("/Volo/Abp/Emailing/Templates/DefaultEmailTemplates/Layout"));

            context.Add(new EmailTemplateDefinition(StandardEmailTemplates.SimpleMessage, defaultCultureName: "en")
                .AddTemplateVirtualFiles("/Volo/Abp/Emailing/Templates/DefaultEmailTemplates/Message"));
        }
    }
}