using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Console
{
    public abstract class ConsoleTemplateBase : TemplateInfo
    {
        protected ConsoleTemplateBase([NotNull] string name) :
            base(name)
        {
        }
    }
}
