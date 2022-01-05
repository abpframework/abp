using Volo.Abp.TextTemplating.Razor.SampleTemplates;

namespace Volo.Abp.TextTemplating.Razor;

public class RazorTestTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.GetOrNull(TestTemplates.WelcomeEmail)?
            .WithVirtualFilePath("/SampleTemplates/WelcomeEmail", false)
            .WithRazorEngine();

        context.GetOrNull(TestTemplates.ForgotPasswordEmail)?
            .WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail.cshtml", true)
            .WithRazorEngine();

        context.GetOrNull(TestTemplates.TestTemplateLayout1)?
            .WithVirtualFilePath("/SampleTemplates/TestTemplateLayout1.cshtml", true)
            .WithRazorEngine();

        context.GetOrNull(TestTemplates.ShowDecimalNumber)?
            .WithVirtualFilePath("/SampleTemplates/ShowDecimalNumber.cshtml", true)
            .WithRazorEngine();

        context.Add(new TemplateDefinition(RazorTestTemplates.TestTemplate).WithVirtualFilePath("/SampleTemplates/TestTemplate.cshtml", true));
    }
}
