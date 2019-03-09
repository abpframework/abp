using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ISingletonDependency
    {
        public abstract void Define(IFeatureDefinitionContext context);
    }
}