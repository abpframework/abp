using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    [Route("api/abp/application-configuration")]
    public class AbpApplicationConfigurationController : AbpController
    {
        private readonly IApplicationConfigurationBuilder _configurationBuilder;

        public AbpApplicationConfigurationController(IApplicationConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        [HttpGet]
        public Task<ApplicationConfigurationDto> Get()
        {
            return _configurationBuilder.GetAsync();
        }
    }
}