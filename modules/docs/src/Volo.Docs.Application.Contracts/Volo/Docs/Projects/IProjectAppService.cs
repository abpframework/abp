using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Docs.Documents;

namespace Volo.Docs.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<ListResultDto<ProjectDto>> GetListAsync();

        Task<ProjectDto> GetAsync(string shortName);

        Task<ListResultDto<VersionInfoDto>> GetVersionsAsync(string shortName);

        Task<string> GetDefaultLanguageCodeAsync(string shortName, string version);

        Task<LanguageConfig> GetLanguageListAsync(string shortName, string version);
    }
}
