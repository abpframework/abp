using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

[Area("abp")]
[RemoteService(Name = "abp")]
[Route("api/abp/api-definition")]
public class AbpApiDefinitionController : AbpController, IRemoteService
{
    protected readonly IApiDescriptionModelProvider ModelProvider;

    public AbpApiDefinitionController(IApiDescriptionModelProvider modelProvider)
    {
        ModelProvider = modelProvider;
    }

    [HttpGet]
    public virtual ApplicationApiDescriptionModel Get(ApplicationApiDescriptionModelRequestDto model)
    {
        return ModelProvider.CreateApiModel(model);
    }
}
