using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.Controllers
{
    [Area("abp")]
    public class ApiDefinitionController : AbpController
    {
        private readonly IApiDescriptionModelProvider _modelProvider;

        public ApiDefinitionController(IApiDescriptionModelProvider modelProvider)
        {
            _modelProvider = modelProvider;
        }

        [HttpGet]
        [Route("api/abp/api-description")]
        public ApplicationApiDescriptionModel Get()
        {
            return _modelProvider.CreateModel();
        }
    }
}
