using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public interface ISettingStore
    {
        Task<string> GetOrNullAsync([NotNull] string name, [CanBeNull] string entityType, [CanBeNull] string entityId);

        Task SetAsync([NotNull] string name, [NotNull] string value, [CanBeNull] string entityType, [CanBeNull] string entityId);

        Task<List<SettingValue>> GetListAsync([CanBeNull] string entityType, [CanBeNull] string entityId);
    }
}
