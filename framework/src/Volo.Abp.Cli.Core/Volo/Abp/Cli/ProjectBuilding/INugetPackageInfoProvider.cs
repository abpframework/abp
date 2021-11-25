using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectModification;

namespace Volo.Abp.Cli.ProjectBuilding;

public interface INugetPackageInfoProvider
{
    Task<NugetPackageInfo> GetAsync(string name);
}
