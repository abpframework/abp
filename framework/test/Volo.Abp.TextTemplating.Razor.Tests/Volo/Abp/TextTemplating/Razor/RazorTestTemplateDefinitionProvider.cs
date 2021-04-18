using Volo.Abp.TextTemplating.Razor.SampleTemplates;

namespace Volo.Abp.TextTemplating.Razor
{
    public class RazorTestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.GetOrNull(TestTemplates.WelcomeEmail)?
                .WithVirtualFilePath("/SampleTemplates/WelcomeEmail", false)
                .WithRenderEngine(RazorTemplateRendererProvider.ProviderName);

            context.GetOrNull(TestTemplates.ForgotPasswordEmail)?
                .WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail.cshtml", true)
                .WithRenderEngine(RazorTemplateRendererProvider.ProviderName);

            context.GetOrNull(TestTemplates.TestTemplateLayout1)?
                .WithVirtualFilePath("/SampleTemplates/TestTemplateLayout1.cshtml", true)
                .WithRenderEngine(RazorTemplateRendererProvider.ProviderName);

            context.GetOrNull(TestTemplates.ShowDecimalNumber)?
                .WithVirtualFilePath("/SampleTemplates/ShowDecimalNumber.cshtml", true)
                .WithRenderEngine(RazorTemplateRendererProvider.ProviderName);

            context.Add(new TemplateDefinition(RazorTestTemplates.TestTemplate).WithVirtualFilePath("/SampleTemplates/TestTemplate.cshtml", true));
        }
    }
}
