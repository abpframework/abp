using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;

namespace Volo.Abp.TextTemplating;

public abstract class TemplateRenderingEngineBase : ITemplateRenderingEngine
{
    public const string CultureContextKey = "abp_culture";

    public const string TextDirectionContextKey = "abp_dir";

    public abstract string Name { get; }

    public virtual bool IsSandboxed => false;

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

    public abstract Task<string> RenderAsync(string templateName, object? model = null, string? cultureName = null, Dictionary<string, object>? globalContext = null);

    /// <summary>
    /// Must be called inside the culture scope of the rendering. Values set by the caller are kept.
    /// </summary>
    protected virtual void SetCultureContext(Dictionary<string, object> globalContext)
    {
        // The invariant culture has an empty name, which is not a valid value for a lang attribute.
        var cultureName = CultureInfo.CurrentUICulture.Name;

        SetCultureContextValue(globalContext, CultureContextKey, cultureName.IsNullOrWhiteSpace() ? "en" : cultureName);
        SetCultureContextValue(globalContext, TextDirectionContextKey, CultureHelper.IsRtl ? "rtl" : "ltr");
    }

    protected virtual void SetCultureContextValue(Dictionary<string, object> globalContext, string key, string value)
    {
        if (!globalContext.TryGetValue(key, out var callerValue))
        {
            globalContext.Add(key, value);
            return;
        }

        // A case insensitive context can hold the value under another casing, while templates look the key
        // up as it is written here, so it is re-inserted with the canonical one.
        globalContext.Remove(key);
        globalContext.Add(key, callerValue);
    }

    protected virtual async Task<string?> GetContentOrNullAsync(TemplateDefinition templateDefinition)
    {
        return await TemplateContentProvider.GetContentOrNullAsync(templateDefinition);
    }

    protected virtual IStringLocalizer? GetLocalizerOrNull(TemplateDefinition templateDefinition)
    {
        if (templateDefinition.LocalizationResourceName != null)
        {
            return StringLocalizerFactory.CreateByResourceName(templateDefinition.LocalizationResourceName);
        }

        return StringLocalizerFactory.CreateDefaultOrNull();
    }
}
