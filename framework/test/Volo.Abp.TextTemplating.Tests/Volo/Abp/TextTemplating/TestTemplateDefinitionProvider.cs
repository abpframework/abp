using Volo.Abp.TextTemplating.Localization;
using Volo.Abp.TextTemplating.Razor;
using Volo.Abp.TextTemplating.Scriban;

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
                )
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.ForgotPasswordEmail,
                    localizationResource: typeof(TestLocalizationSource),
                    layout: TestTemplates.TestTemplateLayout1
                )
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.TestTemplateLayout1,
                    isLayout: true
                )
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.ShowDecimalNumber,
                    localizationResource: typeof(TestLocalizationSource),
                    layout: TestTemplates.TestTemplateLayout1
                )
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.HybridTemplateScriban,
                    localizationResource: typeof(TestLocalizationSource),
                    layout: null
                )
                .WithVirtualFilePath("/SampleTemplates/TestScribanTemplate.tpl", true)
                .WithScribanTemplate()
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.HybridTemplateRazor,
                    localizationResource: typeof(TestLocalizationSource),
                    layout: null
                )
                .WithVirtualFilePath("/SampleTemplates/TestRazorTemplate.cshtml", true)
                .WithRazorTemplate()
            );
        }
    }
}
