using System.Security.Principal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Clients;

public class CurrentClient : ICurrentClient, ITransientDependency
{
    public virtual string Id => _principalAccessor.Principal?.FindClientId();

    public virtual bool IsAuthenticated => Id != null;

    private readonly ICurrentPrincipalAccessor _principalAccessor;

    public CurrentClient(ICurrentPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }
}
