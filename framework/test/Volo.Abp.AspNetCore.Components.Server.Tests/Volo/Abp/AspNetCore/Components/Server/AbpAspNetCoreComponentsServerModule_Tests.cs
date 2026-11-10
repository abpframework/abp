using System.Linq;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AspNetCore.Components.Server;

public abstract class AbpAspNetCoreComponentsServerModule_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    private const string ComponentHubTypeFullName = "Microsoft.AspNetCore.Components.Server.ComponentHub";

    private readonly AbpAspNetCoreAuditingOptions _auditingOptions;

    protected AbpAspNetCoreComponentsServerModule_Tests()
    {
        _auditingOptions = GetRequiredService<IOptions<AbpAspNetCoreAuditingOptions>>().Value;
    }

    [Fact]
    public void Should_Only_Ignore_ComponentHub_For_Hub_Auditing()
    {
        _auditingOptions.IgnoredHubTypes.Single().FullName.ShouldBe(ComponentHubTypeFullName);
    }

    [Fact]
    public void Should_Ignore_Blazor_Urls_For_Auditing()
    {
        _auditingOptions.IgnoredUrls.ShouldContain("/_blazor");
    }
}

public class AbpAspNetCoreComponentsServerModule_BlazorServer_Tests : AbpAspNetCoreComponentsServerModule_Tests<AbpAspNetCoreComponentsServerTestModule>
{
}

public class AbpAspNetCoreComponentsServerModule_BlazorWebApp_Tests : AbpAspNetCoreComponentsServerModule_Tests<AbpAspNetCoreComponentsServerWebAppTestModule>
{
}
