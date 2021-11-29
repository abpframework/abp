using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating.Razor
{
    public static class RazorTemplateDefinitionExtensions
    {
        public static TemplateDefinition WithRazorEngine([NotNull] this TemplateDefinition templateDefinition)
        {
            return templateDefinition.WithRenderEngine(RazorTemplateRenderingEngine.EngineName);
        }
    }
}
