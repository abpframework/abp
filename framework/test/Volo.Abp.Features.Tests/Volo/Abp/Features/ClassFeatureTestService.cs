using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    [RequiresFeature("BooleanTestFeature1")]
    public class ClassFeatureTestService : ITransientDependency
    {
        /* Since this class is used with the class reference,
         * need to virtual keywords, otherwise dynamic proxy can not work.
         */

        [RequiresFeature("BooleanTestFeature2")]
        public virtual Task<int> Feature2Async()
        {
            return Task.FromResult(42);
        }

        public virtual Task NoAdditionalFeatureAsync()
        {
            return Task.CompletedTask;
        }
    }
}