using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring
{
    [Route("api/abp/api-description")]
    public class AbpApiDefinitionController : AbpController
    {
        private readonly IApiDescriptionModelProvider _modelProvider;

        public AbpApiDefinitionController(IApiDescriptionModelProvider modelProvider)
        {
            _modelProvider = modelProvider;
        }

        [HttpGet]
        public ApplicationApiDescriptionModel Get()
        {
            return _modelProvider.CreateApiModel();
        }
    }
}
