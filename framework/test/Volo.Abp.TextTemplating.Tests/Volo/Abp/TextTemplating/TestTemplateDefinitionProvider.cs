namespace Volo.Abp.TextTemplating
{
    public class TestTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context
                .Add(
                    new TemplateDefinition(
                        TestTemplates.TestTemplate1
                    ), new TemplateDefinition(
                        TestTemplates.TestTemplateLayout1
                    )
                );
        }
    }
}
