using Volo.Abp.TextTemplating.Localization;

namespace Volo.Abp.TextTemplating
{
    public class TestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition(
                    TestTemplates.WelcomeEmail,
                    defaultCultureName: "en"
                ).WithVirtualFilePath("/SampleTemplates/WelcomeEmail", false)
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.ForgotPasswordEmail,
                    localizationResource: typeof(TestLocalizationSource),
                    layout: TestTemplates.TestTemplateLayout1
                ).WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail.tpl", true)
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.TestTemplateLayout1,
                    isLayout: true
                ).WithVirtualFilePath("/SampleTemplates/TestTemplateLayout1.tpl", true)
            );
            
            context.Add(
                new TemplateDefinition(
                    TestTemplates.ShowDecimalNumber,
                    localizationResource: typeof(TestLocalizationSource),
                    layout: TestTemplates.TestTemplateLayout1
                ).WithVirtualFilePath("/SampleTemplates/ShowDecimalNumber.tpl", true)
            );
        }
    }
}
