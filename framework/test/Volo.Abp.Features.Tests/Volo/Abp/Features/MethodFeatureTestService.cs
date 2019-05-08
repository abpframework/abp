using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    public class MethodFeatureTestService : ITransientDependency, IMethodFeatureTestService
    {
        /* Since this class is used over an interface (IMethodFeatureTestService),
         * no need to virtual keywords, dynamic proxy can work.
         */

        [RequiresFeature("BooleanTestFeature1")]
        public Task<int> Feature1Async()
        {
            return Task.FromResult(42);
        }

        public Task NonFeatureAsync()
        {
            return Task.CompletedTask;
        }
    }
}
