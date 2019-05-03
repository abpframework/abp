using Volo.Abp.ProjectBuilding.Building;

namespace Volo.Abp.ProjectBuilding
{
    public interface ITemplateInfoProvider
    {
        TemplateInfo GetDefault();

        TemplateInfo Get(string name);
    }
}