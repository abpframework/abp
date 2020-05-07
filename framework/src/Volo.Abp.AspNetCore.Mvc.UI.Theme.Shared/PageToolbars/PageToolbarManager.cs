using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars
{
    public class PageToolbarManager : IPageToolbarManager, ITransientDependency
    {
        protected AbpPageToolbarOptions Options { get; }
        protected IHybridServiceScopeFactory ServiceScopeFactory { get; }

        public PageToolbarManager(
            IOptions<AbpPageToolbarOptions> options,
            IHybridServiceScopeFactory serviceScopeFactory)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
        }

        public virtual async Task<PageToolbarItem[]> GetItemsAsync(string pageName)
        {
            var toolbar = Options.Toolbars.GetOrDefault(pageName);
            if (toolbar == null || !toolbar.Contributors.Any())
            {
                return Array.Empty<PageToolbarItem>();
            }

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = new PageToolbarContributionContext(pageName, scope.ServiceProvider);

                foreach (var contributor in toolbar.Contributors)
                {
                    await contributor.ContributeAsync(context);
                }

                return context.Items.OrderBy(i => i.Order).ToArray();
            }
        }
    }
}