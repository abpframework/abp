using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectModification;

namespace Volo.Abp.Cli.ProjectBuilding;

public interface INpmPackageInfoProvider
{
    Task<NpmPackageInfo> GetAsync(string name);
}
