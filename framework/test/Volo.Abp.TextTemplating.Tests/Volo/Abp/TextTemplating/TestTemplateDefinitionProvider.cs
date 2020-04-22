namespace Volo.Abp.TextTemplating
{
    public class TestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition(
                    TestTemplates.TestTemplate1
                ).AddVirtualFiles("/SampleTemplates/WelcomeEmail")
            );

            context.Add(new TemplateDefinition(
                TestTemplates.TestTemplateLayout1
            ));
        }
    }
}
