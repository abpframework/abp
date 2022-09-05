using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public interface IAbpApplicationLocalizationAppService : IApplicationService
{
    Task<ApplicationLocalizationDto> GetAsync(ApplicationLocalizationRequestDto input);
}