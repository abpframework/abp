using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    public interface IAbpApplicationConfigurationAppService : IApplicationService
    {
        Task<ApplicationConfigurationDto> GetAsync();
    }
}