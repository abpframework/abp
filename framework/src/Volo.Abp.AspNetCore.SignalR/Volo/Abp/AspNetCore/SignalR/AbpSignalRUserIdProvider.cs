using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.SignalR;

public class AbpSignalRUserIdProvider : IUserIdProvider, ITransientDependency
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    private readonly ICurrentUser _currentUser;

    public AbpSignalRUserIdProvider(ICurrentPrincipalAccessor currentPrincipalAccessor, ICurrentUser currentUser)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
        _currentUser = currentUser;
    }

    public virtual string GetUserId(HubConnectionContext connection)
    {
        using (_currentPrincipalAccessor.Change(connection.User))
        {
            return _currentUser.Id?.ToString();
        }
    }
}
