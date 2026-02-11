using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Authentication;

public class AbpAspNetCoreTokenUnauthorizedErrorInfo : IScopedDependency
{
    public string? Error { get; set; }

    public string? ErrorDescription { get; set; }

    public string? ErrorUri { get; set; }
}
