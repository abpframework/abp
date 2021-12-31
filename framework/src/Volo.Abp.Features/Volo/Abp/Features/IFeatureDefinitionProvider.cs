namespace Volo.Abp.Features;

public interface IFeatureDefinitionProvider
{
    void Define(IFeatureDefinitionContext context);
}
