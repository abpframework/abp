using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public interface ISettingManager
    {
        Task<string> GetOrNullAsync(string name, bool fallback = true);

        Task<string> GetOrNullAsync(string name, string entityType, string entityId, bool fallback = true);

        Task<List<SettingValue>> GetAllAsync();

        Task<List<SettingValue>> GetAllAsync(string entityType, string entityId, bool fallback = true);

        Task SetAsync(string name, string value);

        Task SetAsync(string name, string value, string entityType, string entityId);
    }
}