using Volo.Abp.Cli.ProjectBuilding.Templates.Module;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule
{
    public class ModuleTemplate : ModuleTemplateBase
    {
        /// <summary>
        /// "module".
        /// </summary>
        public const string TemplateName = "module";

        public ModuleTemplate()
            : base(TemplateName)
        {
            DocumentUrl = "https://docs.abp.io/en/abp/latest/Startup-Templates/Module";
        }
    }
}
