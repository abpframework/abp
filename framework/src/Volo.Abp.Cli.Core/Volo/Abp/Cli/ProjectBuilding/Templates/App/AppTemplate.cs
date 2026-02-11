using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppTemplate : AppTemplateBase
{
    /// <summary>
    /// "app".
    /// </summary>
    public const string TemplateName = "app";
    
    public const Theme DefaultTheme = Theme.LeptonXLite;

    public AppTemplate()
        : base(TemplateName)
    {
        DocumentUrl = CliConsts.DocsLink + "latest/solution-templates/layered-web-application";
    }
}
