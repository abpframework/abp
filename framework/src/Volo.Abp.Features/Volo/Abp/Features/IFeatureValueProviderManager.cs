using System.Collections.Generic;

namespace Volo.Abp.Features;

public interface IFeatureValueProviderManager
{
    IReadOnlyList<IFeatureValueProvider> ValueProviders { get; }
}