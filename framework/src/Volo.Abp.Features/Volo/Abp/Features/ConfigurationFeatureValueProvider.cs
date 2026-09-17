using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Features;

public class ConfigurationFeatureValueProvider : FeatureValueProvider
{
    public const string ConfigurationNamePrefix = "Features:";

    public const string ProviderName = "C";

    public override string Name => ProviderName;

    protected IConfiguration Configuration { get; }

    public ConfigurationFeatureValueProvider(IFeatureStore featureStore, IConfiguration configuration)
        : base(featureStore)
    {
        Configuration = configuration;
    }

    public override Task<string?> GetOrNullAsync(FeatureDefinition feature)
    {
        return Task.FromResult(Configuration[ConfigurationNamePrefix + feature.Name]);
    }
}
