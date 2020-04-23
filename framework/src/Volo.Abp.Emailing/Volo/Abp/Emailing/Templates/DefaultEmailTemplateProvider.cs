using Volo.Abp.TextTemplating;

namespace Volo.Abp.Emailing.Templates
{
    public class DefaultEmailTemplateProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition(
                    StandardEmailTemplates.DefaultLayout,
                    defaultCultureName: "en",
                    isLayout: true,
                    layout: null
                ).AddVirtualFiles("/Volo/Abp/Emailing/Templates/DefaultEmailTemplates/Layout")
            );

            context.Add(
                new TemplateDefinition(
                    StandardEmailTemplates.SimpleMessage,
                    defaultCultureName: "en"
                ).AddVirtualFiles("/Volo/Abp/Emailing/Templates/DefaultEmailTemplates/Message")
            );
        }
    }
}