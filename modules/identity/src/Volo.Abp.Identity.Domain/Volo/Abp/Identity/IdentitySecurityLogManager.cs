using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.SecurityLog;
using Volo.Abp.Users;

namespace Volo.Abp.Identity;

public class IdentitySecurityLogManager : ITransientDependency
{
    protected ISecurityLogManager SecurityLogManager { get; }
    protected IdentityUserManager UserManager { get; }
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    protected IUserClaimsPrincipalFactory<IdentityUser> UserClaimsPrincipalFactory { get; }
    protected ICurrentUser CurrentUser { get; }

    public IdentitySecurityLogManager(
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

    public async Task SaveAsync(IdentitySecurityLogContext context)
    {
        Action<SecurityLogInfo> securityLogAction = securityLog =>
        {
            securityLog.Identity = context.Identity;
            securityLog.Action = context.Action;

            if (!context.UserName.IsNullOrWhiteSpace())
            {
                securityLog.UserName = context.UserName;
            }

            if (!context.ClientId.IsNullOrWhiteSpace())
            {
                securityLog.ClientId = context.ClientId;
            }

            foreach (var property in context.ExtraProperties)
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
            if (context.UserName.IsNullOrWhiteSpace())
            {
                await SecurityLogManager.SaveAsync(securityLogAction);
            }
            else
            {
                var user = await UserManager.FindByNameAsync(context.UserName);
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
