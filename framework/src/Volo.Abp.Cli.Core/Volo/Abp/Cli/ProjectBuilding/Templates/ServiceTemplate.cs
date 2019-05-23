using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates
{
    public class ServiceTemplate : TemplateInfo
    {
        /// <summary>
        /// "service".
        /// </summary>
        public const string TemplateName = "service";

        public ServiceTemplate()
            : base(TemplateName)
        {

        }
    }
}