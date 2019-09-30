using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public interface IModuleInfoProvider
    {
        Task<ModuleInfo> GetAsync(string name);
    }
}
