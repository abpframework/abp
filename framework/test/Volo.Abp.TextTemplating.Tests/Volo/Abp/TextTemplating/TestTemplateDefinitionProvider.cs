using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating.Localization;
using Volo.Abp.TextTemplating.Razor;
using Volo.Abp.TextTemplating.Scriban;

namespace Volo.Abp.TextTemplating;

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
                localizationResourceName: LocalizationResourceNameAttribute.GetName(typeof(TestLocalizationSource)),
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
                localizationResourceName: LocalizationResourceNameAttribute.GetName(typeof(TestLocalizationSource)),
                layout: TestTemplates.TestTemplateLayout1
            )
        );

        context.Add(
            new TemplateDefinition(
                TestTemplates.HybridTemplateScriban,
                localizationResourceName: LocalizationResourceNameAttribute.GetName(typeof(TestLocalizationSource)),
                layout: null
            )
            .WithVirtualFilePath("/SampleTemplates/TestScribanTemplate.tpl", true)
            .WithScribanEngine()
        );

        context.Add(
            new TemplateDefinition(
                TestTemplates.HybridTemplateRazor,
                localizationResourceName: LocalizationResourceNameAttribute.GetName(typeof(TestLocalizationSource)),
                layout: null
            )
            .WithVirtualFilePath("/SampleTemplates/TestRazorTemplate.cshtml", true)
            .WithRazorEngine()
        );
    }
}
