using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.UI.Navigation;

public interface IMenuConfigurationContext : IServiceProviderAccessor
{
    ApplicationMenu Menu { get; }

    IAuthorizationService AuthorizationService { get; }

    IStringLocalizerFactory StringLocalizerFactory { get; }
}
