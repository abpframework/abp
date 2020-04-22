using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating
{
    public class TestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                new TemplateDefinition(
                    TestTemplates.TestTemplate1
                ).AddContributor(
                    new VirtualFileTemplateContributor("/SampleTemplates/WelcomeEmail")
                )
            );

            context.Add(new TemplateDefinition(
                TestTemplates.TestTemplateLayout1
            ));
        }
    }
}
