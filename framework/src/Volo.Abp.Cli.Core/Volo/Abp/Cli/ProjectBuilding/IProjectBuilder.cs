using System.Threading.Tasks;

namespace Volo.Abp.Cli.ProjectBuilding;

public interface IProjectBuilder
{
    Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args);
}
