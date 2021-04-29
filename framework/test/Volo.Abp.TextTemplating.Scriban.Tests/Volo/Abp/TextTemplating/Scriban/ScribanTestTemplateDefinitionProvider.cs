namespace Volo.Abp.TextTemplating.Scriban
{
    public class ScribanTestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.GetOrNull(TestTemplates.WelcomeEmail)?
                .WithVirtualFilePath("/SampleTemplates/WelcomeEmail", false);

            context.GetOrNull(TestTemplates.ForgotPasswordEmail)?
                .WithVirtualFilePath("/SampleTemplates/ForgotPasswordEmail.tpl", true);

            context.GetOrNull(TestTemplates.TestTemplateLayout1)?
                .WithVirtualFilePath("/SampleTemplates/TestTemplateLayout1.tpl", true);

            context.GetOrNull(TestTemplates.ShowDecimalNumber)?
                .WithVirtualFilePath("/SampleTemplates/ShowDecimalNumber.tpl", true);
        }
    }
}
