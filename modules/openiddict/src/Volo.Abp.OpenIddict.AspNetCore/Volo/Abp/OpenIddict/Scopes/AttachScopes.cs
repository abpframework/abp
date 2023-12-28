using System;
using System.Linq;
using System.Threading.Tasks;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.Scopes;

public class AttachScopes : IOpenIddictServerHandler<OpenIddictServerEvents.HandleConfigurationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.HandleConfigurationRequestContext>()
            .UseSingletonHandler<AttachScopes>()
            .SetOrder(OpenIddictServerHandlers.Discovery.AttachScopes.Descriptor.Order + 1)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    private readonly IOpenIddictScopeRepository _scopeRepository;

    public AttachScopes(IOpenIddictScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public virtual async ValueTask HandleAsync(OpenIddictServerEvents.HandleConfigurationRequestContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var scopes = await _scopeRepository.GetListAsync();
        context.Scopes.UnionWith(scopes.Select(x => x.Name));
    }
}
