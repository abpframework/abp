using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    [Area("abp")]
    [RemoteService(Name = "abp")]
    [Route("api/abp/application-configuration")]
    public class AbpApplicationConfigurationController : AbpController, IAbpApplicationConfigurationAppService
    {
        private readonly IAbpApplicationConfigurationAppService _applicationConfigurationAppService;

        public AbpApplicationConfigurationController(
            IAbpApplicationConfigurationAppService applicationConfigurationAppService)
        {
            _applicationConfigurationAppService = applicationConfigurationAppService;
        }

        [HttpGet]
        public async Task<ApplicationConfigurationDto> GetAsync()
        {
            return await _applicationConfigurationAppService.GetAsync();
        }
    }
}