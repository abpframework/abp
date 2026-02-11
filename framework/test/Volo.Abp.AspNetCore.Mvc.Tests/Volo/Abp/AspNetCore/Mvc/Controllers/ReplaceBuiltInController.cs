using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Controllers;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace Volo.Abp.AspNetCore.Mvc.Controllers;

[Area("abp")]
[RemoteService(Name = "abp")]
[ReplaceControllers(typeof(AbpApplicationConfigurationController), typeof(AbpApplicationLocalizationController))]
public class ReplaceBuiltInController : AbpController
{
    [HttpGet("api/abp/application-configuration")]
    public virtual Task<MyApplicationConfigurationDto> GetAsync(MyApplicationConfigurationRequestOptions options)
    {
        return Task.FromResult(new MyApplicationConfigurationDto()
        {
            Random = options.Random
        });
    }

    [HttpGet("api/abp/application-localization")]
    public virtual Task<MyApplicationLocalizationDto> GetAsync(MyApplicationLocalizationRequestDto input)
    {
        return Task.FromResult(new MyApplicationLocalizationDto()
        {
            Random = input.Random
        });
    }
}

public class MyApplicationConfigurationRequestOptions : ApplicationConfigurationRequestOptions
{
    public string Random { get; set; }
}

public class MyApplicationConfigurationDto : ApplicationConfigurationDto
{
    public string Random { get; set; }
}

public class MyApplicationLocalizationRequestDto : ApplicationLocalizationRequestDto
{
    public string Random { get; set; }
}

public class MyApplicationLocalizationDto : ApplicationLocalizationDto
{
    public string Random { get; set; }
}
