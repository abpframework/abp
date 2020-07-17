using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Security.Claims;
using Volo.Abp.SecurityLog;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class IdentitySecurityLogHandler : ILocalEventHandler<IdentitySecurityLogEvent>, ITransientDependency
    {
        protected ISecurityLogManager SecurityLogManager { get; }
        protected IdentityUserManager UserManager { get; }
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
        protected IUserClaimsPrincipalFactory<IdentityUser> UserClaimsPrincipalFactory { get; }
        protected ICurrentUser CurrentUser { get; }

        public IdentitySecurityLogHandler(
            ISecurityLogManager securityLogManager,
            IdentityUserManager userManager,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
            ICurrentUser currentUser)
        {
            SecurityLogManager = securityLogManager;
            UserManager = userManager;
            CurrentPrincipalAccessor = currentPrincipalAccessor;
            UserClaimsPrincipalFactory = userClaimsPrincipalFactory;
            CurrentUser = currentUser;
        }

        public async Task HandleEventAsync(IdentitySecurityLogEvent eventData)
        {
            Action<SecurityLogInfo> securityLogAction = securityLog =>
            {
                securityLog.Identity = eventData.Identity;
                securityLog.Action = eventData.Action;

                if (securityLog.UserName.IsNullOrWhiteSpace())
                {
                    securityLog.UserName = eventData.UserName;
                }

                if (securityLog.ClientId.IsNullOrWhiteSpace())
                {
                    securityLog.ClientId = eventData.ClientId;
                }

                foreach (var property in eventData.ExtraProperties)
                {
                    securityLog.ExtraProperties[property.Key] = property.Value;
                }
            };

            if (CurrentUser.IsAuthenticated)
            {
                await SecurityLogManager.SaveAsync(securityLogAction);
            }
            else
            {
                if (eventData.UserName.IsNullOrWhiteSpace())
                {
                    await SecurityLogManager.SaveAsync(securityLogAction);
                }
                else
                {
                    var user = await UserManager.FindByNameAsync(eventData.UserName);
                    if (user != null)
                    {
                        using (CurrentPrincipalAccessor.Change(await UserClaimsPrincipalFactory.CreateAsync(user)))
                        {
                            await SecurityLogManager.SaveAsync(securityLogAction);
                        }
                    }
                    else
                    {
                        await SecurityLogManager.SaveAsync(securityLogAction);
                    }
                }
            }
        }
    }
}
