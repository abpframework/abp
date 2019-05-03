using Volo.Abp.SolutionTemplating.Building;

namespace Volo.Abp.ProjectBuilding.Templates
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