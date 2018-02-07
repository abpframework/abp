using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public interface ISettingContributor
    {
        Task<string> GetOrNullAsync(string name, string entityType, string entityId, bool fallback = true);
    }
}