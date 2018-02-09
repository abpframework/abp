using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public interface ISettingValueProvider
    {
        string Name { get; }

        Task<string> GetOrNullAsync([NotNull] SettingDefinition setting, [CanBeNull] string providerKey);

        Task SetAsync([NotNull] SettingDefinition setting, [NotNull] string value, [CanBeNull] string providerKey);

        Task ClearAsync([NotNull] SettingDefinition setting, [CanBeNull] string providerKey);
    }
}