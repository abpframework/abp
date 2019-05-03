using System.Threading.Tasks;

namespace Volo.Abp.ProjectBuilding
{
    public interface IProjectBuilder
    {
        Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args);
    }
}