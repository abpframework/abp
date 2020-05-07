using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring
{
    [Route("api/abp/api-definition")]
    public class AbpApiDefinitionController : AbpController, IRemoteService
    {
        private readonly IApiDescriptionModelProvider _modelProvider;

        public AbpApiDefinitionController(IApiDescriptionModelProvider modelProvider)
        {
            _modelProvider = modelProvider;
        }

        [HttpGet]
        public ApplicationApiDescriptionModel Get(ApplicationApiDescriptionModelRequestDto model)
        {
            return _modelProvider.CreateApiModel(model);
        }
    }
}
