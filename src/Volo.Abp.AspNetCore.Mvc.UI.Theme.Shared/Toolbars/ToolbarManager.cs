using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarManager : IToolbarManager, ITransientDependency
    {
        protected ToolbarOptions Options { get; }
        protected IServiceProvider ServiceProvider { get; }

        public ToolbarManager(IOptions<ToolbarOptions> options, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        public async Task<Toolbar> GetAsync(string name)
        {
            var menu = new Toolbar(name);

            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new ToolbarConfigurationContext(menu, scope.ServiceProvider);

                foreach (var contributor in Options.Contributors)
                {
                    await contributor.ConfigureToolbarAsync(context);
                }
            }

            return menu;
        }
    }
}