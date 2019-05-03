using Volo.Abp.SolutionTemplating.Building;

namespace Volo.Abp.ProjectBuilding
{
    public interface ITemplateInfoProvider
    {
        TemplateInfo GetDefault();

        TemplateInfo Get(string name);
    }
}