using Volo.Abp.Emailing.Localization;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace Volo.Abp.Emailing.Templates;

public class StandardEmailTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition(
                StandardEmailTemplates.Layout,
                displayName: LocalizableString.Create<EmailingResource>("TextTemplate:StandardEmailTemplates.Layout"),
                isLayout: true
            ).WithVirtualFilePath("/Volo/Abp/Emailing/Templates/Layout.tpl", true)
        );

        context.Add(
            new TemplateDefinition(
                StandardEmailTemplates.Message,
                displayName: LocalizableString.Create<EmailingResource>("TextTemplate:StandardEmailTemplates.Message"),
                layout: StandardEmailTemplates.Layout
            ).WithVirtualFilePath("/Volo/Abp/Emailing/Templates/Message.tpl", true)
        );
    }
}
