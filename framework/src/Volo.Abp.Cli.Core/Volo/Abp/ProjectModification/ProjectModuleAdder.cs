using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ProjectModification
{
    public class ProjectModuleAdder : ITransientDependency
    {
        public async Task AddAsync(string projectFile, string moduleName)
        {
            
        }
    }
}