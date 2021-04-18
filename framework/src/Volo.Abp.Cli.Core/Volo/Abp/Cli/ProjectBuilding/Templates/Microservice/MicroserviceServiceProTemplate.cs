namespace Volo.Abp.Cli.ProjectBuilding.Templates.Microservice
{
    public class MicroserviceServiceProTemplate : MicroserviceServiceTemplateBase
    {
        /// <summary>
        /// "microservice-service-pro".
        /// </summary>
        public const string TemplateName = "microservice-service-pro";

        public MicroserviceServiceProTemplate()
            : base(TemplateName)
        {
            DocumentUrl = null; // todo: set this
        }
    }
}
