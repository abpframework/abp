using Volo.Abp.Cli.ProjectBuilding.Templates.Module;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;

public class ModuleTemplate : ModuleTemplateBase
{
    /// <summary>
    /// "module".
    /// </summary>
    public const string TemplateName = "module";

    public ModuleTemplate()
        : base(TemplateName)
    {
        DocumentUrl = "https://abp.io/docs/latest/solution-templates/application-module";
    }
}
