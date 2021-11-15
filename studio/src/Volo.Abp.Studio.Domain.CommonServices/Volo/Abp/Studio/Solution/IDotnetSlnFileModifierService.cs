using System.Threading.Tasks;

namespace Volo.Abp.Studio.Solution
{
    public interface IDotnetSlnFileModifierService
    {
        Task AddProjectAsync(string slnFile, string projectPath, string slnTargetFolder = "src");
    }
}
