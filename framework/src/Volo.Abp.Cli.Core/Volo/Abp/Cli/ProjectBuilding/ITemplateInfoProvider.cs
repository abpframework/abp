using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding;

public interface ITemplateInfoProvider
{
    Task<TemplateInfo> GetDefaultAsync();

    TemplateInfo Get(string name);
}
