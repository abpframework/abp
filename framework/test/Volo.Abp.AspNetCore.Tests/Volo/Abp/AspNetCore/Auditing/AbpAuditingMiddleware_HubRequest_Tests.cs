using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.AspNetCore.Auditing;

public class AbpAuditingMiddleware_HubRequest_Tests
{
    [Fact]
    public async Task Should_Begin_An_Audit_Scope_For_Hub_Requests_By_Default()
    {
        (await IsAuditScopeStartedForHubRequestAsync<NativeSignalRAuditingTestModule>()).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Begin_An_Audit_Scope_For_The_Requests_Of_An_Ignored_Hub_Type()
    {
        (await IsAuditScopeStartedForHubRequestAsync<IgnoredHubTypeTestModule>()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Begin_An_Audit_Scope_For_The_Requests_Of_A_Hub_Derived_From_An_Ignored_Hub_Type()
    {
        (await IsAuditScopeStartedForHubRequestAsync<IgnoredBaseHubTypeTestModule>()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Begin_An_Audit_Scope_For_The_Requests_Of_The_Other_Hubs()
    {
        (await IsAuditScopeStartedForHubRequestAsync<IgnoredOtherHubTypeTestModule>()).ShouldBeTrue();
    }

    private static async Task<bool> IsAuditScopeStartedForHubRequestAsync<TModule>()
        where TModule : IAbpModule
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Host.UseAutofac();
        await builder.AddApplicationAsync<TModule>();

        await using var app = builder.Build();
        await app.InitializeApplicationAsync();
        await app.StartAsync();

        try
        {
            using var client = ((IHost)app).GetTestClient();
            var response = await client.PostAsync("/native-hub/negotiate?negotiateVersion=1", null);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            return bool.Parse(response.Headers.GetValues(NativeSignalRAuditingTestModule.AuditScopeStartedHeaderName).Single());
        }
        finally
        {
            await app.StopAsync();
        }
    }

    public class NativeTestHubBase : Hub
    {
    }

    public class NativeTestHub : NativeTestHubBase
    {
    }

    public class OtherNativeTestHub : Hub
    {
    }

    [DependsOn(typeof(AbpAspNetCoreTestModule))]
    public class NativeSignalRAuditingTestModule : AbpModule
    {
        public const string AuditScopeStartedHeaderName = "X-Audit-Scope-Started";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSignalR();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseRouting();
            app.UseAuditing();
            app.Use(async (httpContext, next) =>
            {
                httpContext.Response.Headers[AuditScopeStartedHeaderName] =
                    (httpContext.RequestServices.GetRequiredService<IAuditingManager>().Current != null).ToString();

                await next();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NativeTestHub>("/native-hub");
            });
        }
    }

    [DependsOn(typeof(NativeSignalRAuditingTestModule))]
    public class IgnoredHubTypeTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreAuditingOptions>(options =>
            {
                options.IgnoredHubTypes.Add(typeof(NativeTestHub));
            });
        }
    }

    [DependsOn(typeof(NativeSignalRAuditingTestModule))]
    public class IgnoredBaseHubTypeTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreAuditingOptions>(options =>
            {
                options.IgnoredHubTypes.Add(typeof(NativeTestHubBase));
            });
        }
    }

    [DependsOn(typeof(NativeSignalRAuditingTestModule))]
    public class IgnoredOtherHubTypeTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreAuditingOptions>(options =>
            {
                options.IgnoredHubTypes.Add(typeof(OtherNativeTestHub));
            });
        }
    }
}
