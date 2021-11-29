using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars
{
    public class SimplePageToolbarContributor : IPageToolbarContributor
    {
        public Type ComponentType { get; }

        public object Argument { get; set; }

        public int Order { get; }

        public string RequiredPolicyName { get; }

        public SimplePageToolbarContributor(
            Type componentType,
            object argument = null,
            int order = 0,
            string requiredPolicyName = null)
        {
            ComponentType = componentType;
            Argument = argument;
            Order = order;
            RequiredPolicyName = requiredPolicyName;
        }

        public async Task ContributeAsync(PageToolbarContributionContext context)
        {
            if(await ShouldAddComponentAsync(context))
            {
                context.Items.Add(new PageToolbarItem(ComponentType, Argument, Order));
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
}