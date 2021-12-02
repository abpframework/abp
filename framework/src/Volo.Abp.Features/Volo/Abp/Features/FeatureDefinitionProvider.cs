using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features;

public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ITransientDependency
{
    public abstract void Define(IFeatureDefinitionContext context);
}
