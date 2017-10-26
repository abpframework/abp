using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    public class ApplicationConfigurationController : AbpController
    {
        private readonly IApplicationConfigurationBuilder _configurationBuilder;

        public ApplicationConfigurationController(IApplicationConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        public async Task<JsonResult> GetAll()
        {
            //TODO: 1. Make an API
            //TODO: 2. Make a script generation controller

            var userConfig = await _configurationBuilder.Get();
            return Json(userConfig);
        }
    }
}
