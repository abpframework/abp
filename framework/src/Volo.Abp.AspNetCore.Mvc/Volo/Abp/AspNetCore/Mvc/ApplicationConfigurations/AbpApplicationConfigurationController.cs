using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    [Route("api/abp/application-configuration")]
    public class AbpApplicationConfigurationController : AbpController
    {
        private readonly IAbpApplicationConfigurationAppService _applicationConfigurationAppService;

        public AbpApplicationConfigurationController(
            IAbpApplicationConfigurationAppService applicationConfigurationAppService)
        {
            _applicationConfigurationAppService = applicationConfigurationAppService;
        }

        [HttpGet]
        public Task<ApplicationConfigurationDto> GetAsync()
        {
            return _applicationConfigurationAppService.GetAsync();
        }
    }
}