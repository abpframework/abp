using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

public class SimplePageToolbarContributor : IPageToolbarContributor
{
    public Type ComponentType { get; }

    public Dictionary<string, object> Arguments { get; set; }

    public int Order { get; }

    public string RequiredPolicyName { get; }

    public SimplePageToolbarContributor(
        Type componentType,
        Dictionary<string, object> arguments = null,
        int order = 0,
        string requiredPolicyName = null)
    {
        ComponentType = componentType;
        Arguments = arguments;
        Order = order;
        RequiredPolicyName = requiredPolicyName;
    }

    public async Task ContributeAsync(PageToolbarContributionContext context)
    {
        if (await ShouldAddComponentAsync(context))
        {
            context.Items.Add(new PageToolbarItem(ComponentType, Arguments, Order));
        }
    }

    protected virtual async Task<bool> ShouldAddComponentAsync(PageToolbarContributionContext context)
    {
        if (RequiredPolicyName != null)
        {
            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();
            if (!await authorizationService.IsGrantedAsync(RequiredPolicyName))
            {
                return false;
            }
        }

        return true;
    }
}
