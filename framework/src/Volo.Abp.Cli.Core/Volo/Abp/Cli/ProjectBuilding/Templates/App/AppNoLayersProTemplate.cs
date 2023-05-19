using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppNoLayersProTemplate : AppNoLayersTemplateBase
{
    /// <summary>
    /// "app-nolayers-pro".
    /// </summary>
    public const string TemplateName = "app-nolayers-pro";
    
    public const Theme DefaultTheme = Theme.LeptonX;

    public AppNoLayersProTemplate()
        : base(TemplateName)
    {
        //TODO: Change URL
        //DocumentUrl = CliConsts.DocsLink + "/en/abp/latest/Startup-Templates/Application";
    }
}
