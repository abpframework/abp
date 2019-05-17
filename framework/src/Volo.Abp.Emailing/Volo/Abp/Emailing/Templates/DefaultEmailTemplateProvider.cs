using Volo.Abp.Emailing.Templates.Virtual;

namespace Volo.Abp.Emailing.Templates
{
    public class DefaultEmailTemplateProvider : EmailTemplateDefinitionProvider
    {
        public override void Define(IEmailTemplateDefinitionContext context)
        {
            context.Add(new EmailTemplateDefinition(StandardEmailTemplates.DefaultLayout, isLayout: true, layout: null)
                .SetVirtualFilePath("/Volo/Abp/Emailing/Templates/DefaultLayout.html"));

            context.Add(new EmailTemplateDefinition(StandardEmailTemplates.SimpleMessage)
                .SetVirtualFilePath("/Volo/Abp/Emailing/Templates/SimpleMessageTemplate.html"));
        }
    }
}