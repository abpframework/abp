using Volo.Abp.TextTemplating.Razor.SampleTemplates;

namespace Volo.Abp.TextTemplating.Razor
{
    public class RazorTestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.GetOrNull(TestTemplates.WelcomeEmail)?
                .WithVirtualFilePath("/SampleTemplates/WelcomeEmail", false)
                .WithRazorTemplate();

            context.GetOrNull(TestTemplates.ForgotPasswordEmail)?
                .WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail.cshtml", true)
                .WithRazorTemplate();

            context.GetOrNull(TestTemplates.TestTemplateLayout1)?
                .WithVirtualFilePath("/SampleTemplates/TestTemplateLayout1.cshtml", true)
                .WithRazorTemplate();

            context.GetOrNull(TestTemplates.ShowDecimalNumber)?
                .WithVirtualFilePath("/SampleTemplates/ShowDecimalNumber.cshtml", true)
                .WithRazorTemplate();

            context.Add(new TemplateDefinition(RazorTestTemplates.TestTemplate).WithVirtualFilePath("/SampleTemplates/TestTemplate.cshtml", true));
        }
    }
}
