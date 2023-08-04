using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;

public class MicroserviceProTemplate : MicroserviceTemplateBase
{
    /// <summary>
    /// "microservice-pro".
    /// </summary>
    public const string TemplateName = "microservice-pro";

    public const Theme DefaultTheme = Theme.LeptonX;

    public MicroserviceProTemplate()
        : base(TemplateName)
    {
        DocumentUrl = null; // todo: set this
    }
}
