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
        public virtual int Feature2()
        {
            return 42;
        }

        public virtual void NoAdditionalFeature()
        {
            
        }
    }
}