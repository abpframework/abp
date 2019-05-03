using System.Threading.Tasks;
using Volo.Abp.SolutionTemplating;

namespace Volo.Abp.ProjectBuilding
{
    public interface IProjectBuilder
    {
        Task<ProjectBuildResult> BuildAsync(ProjectBuildArgs args);
    }
}