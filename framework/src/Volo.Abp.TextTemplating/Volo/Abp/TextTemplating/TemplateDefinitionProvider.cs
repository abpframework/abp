using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating
{
    public abstract class TemplateDefinitionProvider : ITemplateDefinitionProvider, ITransientDependency
    {
        public abstract void Define(ITemplateDefinitionContext context);
    }
}