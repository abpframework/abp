namespace Volo.Abp.Cli.ProjectBuilding.Templates.Wpf;

public class WpfTemplate : WpfTemplateBase
{
    /// <summary>
    /// "wpf".
    /// </summary>
    public const string TemplateName = "wpf";

    public WpfTemplate()
        : base(TemplateName)
    {
        DocumentUrl = CliConsts.DocsLink + "latest/get-started/WPF";
    }
}
