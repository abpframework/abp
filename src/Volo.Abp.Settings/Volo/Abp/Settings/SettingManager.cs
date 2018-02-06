using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public class SettingManager : ISettingManager
    {
        public Task<string> GetOrNullAsync(string name)
        {
            return GetOrNullAsync(name, null, null);
        }

        public Task<string> GetOrNullAsync(string name, string entityType, string entityId, bool fallback = true)
        {
            var contributors = new List<ISettingContributor>();

            foreach (var contributor in contributors)
            {
                var value = contributor.GetOrNull(name);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        public Task<List<SettingValue>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SettingValue>> GetAllAsync(string entityType, string entityId, bool fallback = true)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAsync(string name, string value, string entityType, string entityId)
        {
            throw new System.NotImplementedException();
        }
    }
}