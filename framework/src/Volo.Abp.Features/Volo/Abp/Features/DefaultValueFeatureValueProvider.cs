using System.Threading.Tasks;

namespace Volo.Abp.Features;

public class DefaultValueFeatureValueProvider : FeatureValueProvider //TODO: Directly implement IFeatureValueProvider
{
    public const string ProviderName = "D";

    public override string Name => ProviderName;

    public DefaultValueFeatureValueProvider(IFeatureStore settingStore)
        : base(settingStore)
    {

    }

    public override Task<string> GetOrNullAsync(FeatureDefinition setting)
    {
        return Task.FromResult(setting.DefaultValue);
    }
}
