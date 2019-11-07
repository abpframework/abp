using System.Threading.Tasks;

namespace Volo.Abp.Cli.Licensing
{
    public interface IApiKeyService
    {
        Task<DeveloperApiKeyResult> GetApiKeyOrNullAsync();
    }
}