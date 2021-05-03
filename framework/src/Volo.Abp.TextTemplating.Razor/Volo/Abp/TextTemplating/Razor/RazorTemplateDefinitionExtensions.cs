using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating.Razor
{
    public static class RazorTemplateDefinitionExtensions
    {
        public static TemplateDefinition WithRazorTemplate([NotNull] this TemplateDefinition templateDefinition)
        {
            return templateDefinition.WithRenderEngine(RazorTemplateRenderingEngine.EngineName);
        }
    }
}
