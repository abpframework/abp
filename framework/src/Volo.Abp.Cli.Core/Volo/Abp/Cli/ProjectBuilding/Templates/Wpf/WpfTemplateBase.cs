using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Wpf;

public class WpfTemplateBase : TemplateInfo
{
    protected WpfTemplateBase([NotNull] string name) :
        base(name)
    {
    }
}
