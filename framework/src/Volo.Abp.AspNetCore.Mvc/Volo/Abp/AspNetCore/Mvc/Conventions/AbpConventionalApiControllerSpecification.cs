using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class AbpConventionalApiControllerSpecification : IApiControllerSpecification
{
    private readonly AbpAspNetCoreMvcOptions _options;

    public AbpConventionalApiControllerSpecification(IOptions<AbpAspNetCoreMvcOptions> options)
    {
        _options = options.Value;
    }

    public bool IsSatisfiedBy(ControllerModel controller)
    {
        var configuration = _options
            .ConventionalControllers
            .ConventionalControllerSettings
            .GetSettingOrNull(controller.ControllerType.AsType());

        return configuration != null;
    }
}
