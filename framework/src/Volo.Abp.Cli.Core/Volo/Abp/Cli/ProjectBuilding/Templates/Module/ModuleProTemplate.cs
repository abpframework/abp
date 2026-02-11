using Volo.Abp.Cli.ProjectBuilding.Templates.Module;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;

public class ModuleProTemplate : ModuleTemplateBase
{
    /// <summary>
    /// "module".
    /// </summary>
    public const string TemplateName = "module-pro";

    public ModuleProTemplate()
        : base(TemplateName)
    {
        DocumentUrl = "https://abp.io/docs/latest/solution-templates/application-module";
    }
}
