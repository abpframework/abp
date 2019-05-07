using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ProjectModification
{
    public class SolutionModuleAdder : ITransientDependency
    {
        public async Task AddAsync(string solutionFile, string moduleName)
        {

        }
    }
}