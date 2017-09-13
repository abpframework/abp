using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.Controllers
{
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
            //TODO: It can not create methods if there are overloads of same action in a controller!

            return _modelProvider.CreateModel();
        }
    }
}
