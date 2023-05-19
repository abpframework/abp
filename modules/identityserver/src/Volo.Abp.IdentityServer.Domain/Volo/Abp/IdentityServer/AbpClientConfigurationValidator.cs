using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Validation;

namespace Volo.Abp.IdentityServer;

public class AbpClientConfigurationValidator : DefaultClientConfigurationValidator
{
    public AbpClientConfigurationValidator(IdentityServerOptions options)
        : base(options)
    {
    }

    protected override Task ValidateAllowedCorsOriginsAsync(ClientConfigurationValidationContext context)
    {
        context.Client.AllowedCorsOrigins = context.Client
            .AllowedCorsOrigins.Select(x => x.Replace("{0}.", string.Empty, StringComparison.OrdinalIgnoreCase))
            .ToHashSet();

        return base.ValidateAllowedCorsOriginsAsync(context);
    }
}
