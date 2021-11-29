using System.Threading.Tasks;

namespace Volo.Abp.Studio.Solution
{
    public interface ISolutionFileModuleAdder
    {
        Task AddAsync(string TargetModule, string ModuleName);
    }
}
