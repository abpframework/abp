using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class AbpSignalRUserIdProvider : IUserIdProvider, ITransientDependency
    {
        public ICurrentUser CurrentUser { get; }
        
        public AbpSignalRUserIdProvider(ICurrentUser currentUser)
        {
            CurrentUser = currentUser;
        }

        public virtual string GetUserId(HubConnectionContext connection)
        {
            return CurrentUser.Id?.ToString();
        }
    }
}
