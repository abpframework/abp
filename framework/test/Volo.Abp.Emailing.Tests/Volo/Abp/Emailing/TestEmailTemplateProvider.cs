using Volo.Abp.Emailing.Templates;
using Volo.Abp.Emailing.Templates.Virtual;

namespace Volo.Abp.Emailing
{
    public class TestEmailTemplateProvider : EmailTemplateDefinitionProvider
    {
        public override void Define(IEmailTemplateDefinitionContext context)
        {
            context.Add(new EmailTemplateDefinition("template1")
                .SetVirtualFilePath("/Volo/Abp/Emailing/TestTemplates/template1.html"));
        }
    }
}