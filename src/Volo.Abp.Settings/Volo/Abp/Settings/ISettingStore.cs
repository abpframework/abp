using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public interface ISettingStore
    {
        Task<string> GetOrNullAsync([NotNull] string name, [CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task SetAsync([NotNull] string name, [NotNull] string value, [CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task<List<SettingValue>> GetListAsync([CanBeNull] string providerName, [CanBeNull] string providerKey);

        Task DeleteAsync([NotNull] string name, [CanBeNull]string providerName, [CanBeNull]string providerKey);
    }
}
