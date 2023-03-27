using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

[Area("abp")]
[RemoteService(Name = "abp")]
[Route("api/abp/api-definition")]
public class AbpApiDefinitionController : AbpController, IRemoteService
{
    public const string CacheProfileName = $"{nameof(AbpApiDefinitionController)}.{nameof(AbpApiDefinitionController.Get)}";

    protected readonly IApiDescriptionModelProvider ModelProvider;

    public AbpApiDefinitionController(IApiDescriptionModelProvider modelProvider)
    {
        ModelProvider = modelProvider;
    }

    [HttpGet]
    [ResponseCache(CacheProfileName = CacheProfileName)]
    public virtual ApplicationApiDescriptionModel Get(ApplicationApiDescriptionModelRequestDto model)
    {
        return ModelProvider.CreateApiModel(model);
    }
}
