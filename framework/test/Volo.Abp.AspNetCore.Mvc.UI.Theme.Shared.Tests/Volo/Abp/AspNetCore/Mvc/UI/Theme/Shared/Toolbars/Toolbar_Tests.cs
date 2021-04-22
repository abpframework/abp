using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Tests.Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class Toolbar_Tests : AbpAspNetCoreMvcUiThemeSharedTestBase
    {
        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new MyToolbarContributor());
                options.Contributors.Add(new MyToolbarContributor2());
            });

            var claims = new List<Claim>() {
                new Claim(AbpClaimTypes.UserId, "1fcf46b2-28c3-48d0-8bac-fa53268a2775"),
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var principalAccessor = Substitute.For<ICurrentPrincipalAccessor>();
            principalAccessor.Principal.Returns(ci => claimsPrincipal);
            Thread.CurrentPrincipal = claimsPrincipal;

            var themeManager = Substitute.For<IThemeManager>();
            themeManager.CurrentTheme.Returns(x => null);
            services.Replace(ServiceDescriptor.Singleton<IThemeManager>(themeManager));
        }

        [Fact]
        public void AbpToolbarOptions_Should_Contain_Contributors()
        {
            var options = GetRequiredService<IOptions<AbpToolbarOptions>>().Value;
            options.Contributors.Count.ShouldBe(2);
            options.Contributors.ShouldContain(x => x.GetType() == typeof(MyToolbarContributor));
            options.Contributors.ShouldContain(x => x.GetType() == typeof(MyToolbarContributor2));
        }

        [Fact]
        public async Task ToolbarManager_Should_Return_Toolbar()
        {
            var toolbarManager = GetRequiredService<ToolbarManager>();
            var toolbar = await toolbarManager.GetAsync(StandardToolbars.Main);

            toolbar.Items.Count.ShouldBe(3);
            toolbar.Items[0].ComponentType.ShouldBe(typeof(MyComponent1));
            toolbar.Items[1].ComponentType.ShouldBe(typeof(MyComponent3));
            toolbar.Items[2].ComponentType.ShouldBe(typeof(MyComponent4));
        }

        public class MyToolbarContributor : IToolbarContributor
        {
            public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
            {
                if (context.Toolbar.Name != StandardToolbars.Main)
                {
                    return Task.CompletedTask;
                }

                context.Toolbar.Items.Add(new ToolbarItem(typeof(MyComponent1)).RequirePermissions("MyComponent1"));
                context.Toolbar.Items.Add(new ToolbarItem(typeof(MyComponent2)).RequirePermissions("MyComponent2"));

                return Task.CompletedTask;
            }
        }

        public class MyToolbarContributor2 : IToolbarContributor
        {
            public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
            {
                if (context.Toolbar.Name != StandardToolbars.Main)
                {
                    return Task.CompletedTask;
                }

                context.Toolbar.Items.Add(new ToolbarItem(typeof(MyComponent3)).RequirePermissions("MyComponent3"));
                context.Toolbar.Items.Add(new ToolbarItem(typeof(MyComponent4)));

                return Task.CompletedTask;
            }
        }

        public class MyComponent1 : AbpViewComponent
        {
            public IViewComponentResult InvokeAsync()
            {
                return Content("MyComponent1");
            }
        }

        public class MyComponent2 : AbpViewComponent
        {
            public IViewComponentResult InvokeAsync()
            {
                return Content("MyComponent2");
            }
        }

        public class MyComponent3 : AbpViewComponent
        {
            public IViewComponentResult InvokeAsync()
            {
                return Content("MyComponent3");
            }
        }

        public class MyComponent4 : AbpViewComponent
        {
            public IViewComponentResult InvokeAsync()
            {
                return Content("MyComponent4");
            }
        }
    }
}
