namespace Volo.Abp.TextTemplating;

public interface ITemplateDefinitionProvider
{
    void PreDefine(ITemplateDefinitionContext context);

    void Define(ITemplateDefinitionContext context);

    void PostDefine(ITemplateDefinitionContext context);
}
