namespace Volo.Abp.TextTemplating
{
    public interface ITemplateDefinitionContext
    {
        TemplateDefinition GetOrNull(string name);

        void Add(params TemplateDefinition[] definitions);
    }
}