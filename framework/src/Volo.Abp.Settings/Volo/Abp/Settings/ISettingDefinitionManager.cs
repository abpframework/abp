using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings;

public interface ISettingDefinitionManager
{
    [ItemNotNull]
    Task<SettingDefinition> GetAsync([NotNull] string name);

    [ItemNotNull]
    Task<IReadOnlyList<SettingDefinition>> GetAllAsync();

    [ItemCanBeNull]
    Task<SettingDefinition?> GetOrNullAsync([NotNull] string name);
}
