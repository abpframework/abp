using System.Collections.Generic;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Volo.Abp.Features;

public interface IFeatureChecker
{
    Task<string?> GetOrNullAsync([NotNull] string name);

    Task<bool> IsEnabledAsync(string name);

    Task<Dictionary<string, bool>> IsEnabledAsync([NotNull] string[] names);
}
