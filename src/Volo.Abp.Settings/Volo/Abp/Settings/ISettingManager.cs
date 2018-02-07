using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public interface ISettingManager
    {
        Task<string> GetOrNullAsync([NotNull]string name);

        Task<string> GetOrNullAsync([NotNull]string name, [NotNull] string entityType, [CanBeNull] string entityId, bool fallback = true);

        Task<List<SettingValue>> GetAllAsync();

        Task<List<SettingValue>> GetAllAsync([NotNull] string entityType, [CanBeNull] string entityId, bool fallback = true);

        Task SetAsync([NotNull] string name, [CanBeNull] string value, bool forceToSet = false);

        Task SetAsync([NotNull] string name, [CanBeNull] string value, [NotNull] string entityType, [CanBeNull] string entityId, bool forceToSet = false);
    }
}