using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating;

public interface ITemplateRenderingEngine
{
    string Name { get; }

    /// <summary>
    /// Indicates whether this engine renders template content in a sandboxed way that
    /// prevents the content from accessing the host runtime (filesystem, environment,
    /// arbitrary .NET APIs, etc.).
    /// <para>
    /// Sandboxed engines (e.g. Scriban, Liquid) interpret templates as a restricted DSL
    /// without .NET interop. Non-sandboxed engines (e.g. Razor) compile templates into
    /// fully-trusted .NET code that runs with the same privileges as the host process.
    /// </para>
    /// <para>
    /// Implementations are required to declare this explicitly. The recommended
    /// secure-by-default value is <c>false</c>: any engine that doesn't have a clear
    /// sandboxing story should return <c>false</c> so callers such as the
    /// TextTemplateManagement module treat its templates as requiring elevated trust
    /// to edit. <see cref="TemplateRenderingEngineBase"/> provides a virtual default
    /// of <c>false</c> for engines deriving from it.
    /// </para>
    /// </summary>
    bool IsSandboxed { get; }

    /// <summary>
    /// Renders a text template.
    /// </summary>
    /// <param name="templateName">The template name</param>
    /// <param name="model">An optional model object that is used in the template</param>
    /// <param name="cultureName">Culture name. Uses the <see cref="CultureInfo.CurrentUICulture"/> if not specified</param>
    /// <param name="globalContext">A dictionary which can be used to import global objects to the template</param>
    /// <returns></returns>
    Task<string> RenderAsync(
        [NotNull] string templateName,
        object? model = null,
        string? cultureName = null,
        Dictionary<string, object>? globalContext = null
    );
}
