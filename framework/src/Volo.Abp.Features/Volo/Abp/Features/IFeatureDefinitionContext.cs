namespace Volo.Abp.Features
{
    public interface IFeatureDefinitionContext
    {
        FeatureDefinition GetOrNull(string name);

        void Add(params FeatureDefinition[] definitions);
    }
}