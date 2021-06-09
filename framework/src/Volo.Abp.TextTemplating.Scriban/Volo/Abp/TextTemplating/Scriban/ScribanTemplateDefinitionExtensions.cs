using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating.Scriban
{
    public static class ScribanTemplateDefinitionExtensions
    {
        public static TemplateDefinition WithScribanEngine([NotNull] this TemplateDefinition templateDefinition)
        {
            return templateDefinition.WithRenderEngine(ScribanTemplateRenderingEngine.EngineName);
        }
    }
}
