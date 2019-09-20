using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public interface IModuleInfoProvider
    {
        Task<ModuleInfo> GetAsync(string name);
    }
}
