using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public interface ITemplateInfoProvider
    {
        TemplateInfo GetDefault();

        TemplateInfo Get(string name);
    }
}