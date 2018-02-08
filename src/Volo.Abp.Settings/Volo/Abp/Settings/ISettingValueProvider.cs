using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public interface ISettingValueProvider
    {
        string EntityType { get; }

        Task<string> GetOrNullAsync([NotNull] SettingDefinition setting, [CanBeNull] string entityId);

        Task SetAsync([NotNull] SettingDefinition setting, [NotNull] string value, [CanBeNull] string entityId);

        Task ClearAsync([NotNull] SettingDefinition setting, [CanBeNull] string entityId);
    }
}