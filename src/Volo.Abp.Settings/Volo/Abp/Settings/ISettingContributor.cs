using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public interface ISettingContributor
    {
        string EntityType { get; }

        Task<string> GetOrNullAsync(string name, bool fallback);

        Task<string> GetOrNullAsync(string name, string entityId, bool fallback = true);
    }
}