namespace Volo.Abp.Cli.ProjectBuilding.Templates.Maui;

public class MauiTemplate : MauiTemplateBase
{
    /// <summary>
    /// "maui".
    /// </summary>
    public const string TemplateName = "maui";
    
    public MauiTemplate() 
        : base(TemplateName)
    {
        DocumentUrl = CliConsts.DocsLink + "latest/get-started/MAUI";
    }
}