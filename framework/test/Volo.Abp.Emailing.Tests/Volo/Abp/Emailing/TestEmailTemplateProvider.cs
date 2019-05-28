using Volo.Abp.Emailing.Localization;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Emailing.Templates.VirtualFiles;

namespace Volo.Abp.Emailing
{
    public class TestEmailTemplateProvider : EmailTemplateDefinitionProvider
    {
        public override void Define(IEmailTemplateDefinitionContext context)
        {
            var template1 = new EmailTemplateDefinition("template1", defaultCultureName: "en", layout: null)
                .AddTemplateVirtualFiles("/Volo/Abp/Emailing/TestTemplates/Template1");
            context.Add(template1);

            var template2 = new EmailTemplateDefinition("template2", layout: StandardEmailTemplates.DefaultLayout)
                .AddTemplateVirtualFiles("/Volo/Abp/Emailing/TestTemplates/Template2");
            context.Add(template2);

            var template3 = new EmailTemplateDefinition("template3", layout: null, singleTemplateFile: true, localizationResource: typeof(AbpEmailingTestResource))
                .AddTemplateVirtualFile("/Volo/Abp/Emailing/TestTemplates/Template3/Template.tpl");
            context.Add(template3);
        }
    }
}