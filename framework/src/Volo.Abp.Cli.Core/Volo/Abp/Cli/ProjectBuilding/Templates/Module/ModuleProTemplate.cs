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
        //TODO: uncomment next line after document is ready
        //DocumentUrl = "https://docs.abp.io/en/commercial/latest/Startup-Templates/Module";
    }
}
