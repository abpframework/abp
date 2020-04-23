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
                ).AddVirtualFiles("/SampleTemplates/WelcomeEmail")
            );

            context.Add(
                new TemplateDefinition(
                    TestTemplates.ForgotPasswordEmail
                ).AddVirtualFiles("/SampleTemplates/ForgotPasswordEmail.tpl")
            );

            context.Add(new TemplateDefinition(
                TestTemplates.TestTemplateLayout1
            ));
        }
    }
}
