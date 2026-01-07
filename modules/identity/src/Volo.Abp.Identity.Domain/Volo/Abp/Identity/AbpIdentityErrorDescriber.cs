using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity.Localization;

namespace Volo.Abp.Identity;

[Dependency(ServiceLifetime.Scoped, ReplaceServices = true)]
[ExposeServices(typeof(IdentityErrorDescriber))]
public class AbpIdentityErrorDescriber : IdentityErrorDescriber
{
    protected IStringLocalizer<IdentityResource> Localizer { get; }

    public AbpIdentityErrorDescriber(IStringLocalizer<IdentityResource> localizer)
    {
        Localizer = localizer;
    }

    public override IdentityError InvalidUserName([CanBeNull] string userName)
    {
        return new IdentityError
        {
            Code = nameof(InvalidUserName),
            Description = Localizer["Volo.Abp.Identity:InvalidUserName", userName ?? ""]
        };
    }
}
