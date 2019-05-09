using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ProjectModification
{
    public class SolutionModuleAdder : ITransientDependency
    {
        public virtual async Task AddAsync([NotNull] string solutionFile, [NotNull] string moduleName)
        {
            Check.NotNull(solutionFile, nameof(solutionFile));
            Check.NotNull(moduleName, nameof(moduleName));


        }
    }
}