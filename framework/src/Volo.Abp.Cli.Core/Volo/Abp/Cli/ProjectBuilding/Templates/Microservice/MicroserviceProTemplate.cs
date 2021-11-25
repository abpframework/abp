namespace Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;

public class MicroserviceProTemplate : MicroserviceTemplateBase
{
    /// <summary>
    /// "microservice-pro".
    /// </summary>
    public const string TemplateName = "microservice-pro";

    public MicroserviceProTemplate()
        : base(TemplateName)
    {
        DocumentUrl = null; // todo: set this
    }
}
