using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings;

public interface ISettingValueProvider
{
    string Name { get; }

    Task<string> GetOrNullAsync([NotNull] SettingDefinition setting);

    Task<List<SettingValue>> GetAllAsync([NotNull] SettingDefinition[] settings);
}
