using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.TextTemplating;

public abstract class TemplateRenderingEngineBase : ITemplateRenderingEngine
{
    public abstract string Name { get; }

    protected readonly ITemplateDefinitionManager TemplateDefinitionManager;
    protected readonly ITemplateContentProvider TemplateContentProvider;
    protected readonly IStringLocalizerFactory StringLocalizerFactory;

    public TemplateRenderingEngineBase(
        ITemplateDefinitionManager templateDefinitionManager,
        ITemplateContentProvider templateContentProvider,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        TemplateDefinitionManager = templateDefinitionManager;
        TemplateContentProvider = templateContentProvider;
        StringLocalizerFactory = stringLocalizerFactory;
    }

    public abstract Task<string> RenderAsync(string templateName, object model = null, string cultureName = null, Dictionary<string, object> globalContext = null);

    protected virtual async Task<string> GetContentOrNullAsync(TemplateDefinition templateDefinition)
    {
        return await TemplateContentProvider.GetContentOrNullAsync(templateDefinition);
    }

    protected virtual IStringLocalizer GetLocalizerOrNull(TemplateDefinition templateDefinition)
    {
        if (templateDefinition.LocalizationResource != null)
        {
            return StringLocalizerFactory.Create(templateDefinition.LocalizationResource);
        }

        return StringLocalizerFactory.CreateDefaultOrNull();
    }
}
