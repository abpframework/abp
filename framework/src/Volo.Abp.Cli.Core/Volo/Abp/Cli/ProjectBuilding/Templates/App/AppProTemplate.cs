using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppProTemplate : AppTemplateBase
{
    /// <summary>
    /// "app-pro".
    /// </summary>
    public const string TemplateName = "app-pro";
    
    public const Theme DefaultTheme = Theme.LeptonX;

    public AppProTemplate()
        : base(TemplateName)
    {
        DocumentUrl = CliConsts.DocsLink + "/en/commercial/latest";
    }
}
