using Volo.Abp.TextTemplating;

namespace Volo.Abp.Emailing.Templates
{
    public class DefaultEmailTemplateProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition(
                    StandardEmailTemplates.Layout,
                    defaultCultureName: "en",
                    isLayout: true
                ).WithVirtualFilePath("/Volo/Abp/Emailing/Templates/Layout")
            );

            context.Add(
                new TemplateDefinition(
                    StandardEmailTemplates.Message,
                    defaultCultureName: "en",
                    layout: StandardEmailTemplates.Layout
                ).WithVirtualFilePath("/Volo/Abp/Emailing/Templates/Message")
            );
        }
    }
}