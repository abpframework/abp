using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings;

public interface ISettingStore
{
    Task<string?> GetOrNullAsync(
        [NotNull] string name,
        string? providerName,
        string? providerKey
    );

    Task<List<SettingValue>> GetAllAsync(
        [NotNull] string[] names,
        string? providerName,
        string? providerKey
    );
}
