using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Studio.ModuleInstalling.Options
{
    public interface IModuleInstallingOptionProvider
    {
        Task<List<ModuleInstallingOption>> GetAsync();
    }
}
