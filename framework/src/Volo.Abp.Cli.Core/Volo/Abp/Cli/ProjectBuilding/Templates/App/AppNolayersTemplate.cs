namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppNolayersTemplate : AppTemplateBase
{
    /// <summary>
    /// "app".
    /// </summary>
    public const string TemplateName = "app-nolayers";

    public AppNolayersTemplate()
        : base(TemplateName)
    {
        //TODO: Change URL
        DocumentUrl = CliConsts.DocsLink + "/en/abp/latest/Startup-Templates/Application";
    }
}
