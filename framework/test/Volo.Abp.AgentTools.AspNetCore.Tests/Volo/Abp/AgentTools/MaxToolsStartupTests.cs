using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AgentTools.Sample;
using Xunit;

namespace Volo.Abp.AgentTools;

// The guard runs at application initialization, not only on the first tool list.
public class MaxToolsStartupTests
{
    [Fact]
    public void Host_Start_Fails_When_Included_Tools_Exceed_MaxTools()
    {
        var exception = Should.Throw<Exception>(() => new TooManyToolsHost());

        var messages = "";
        for (var e = exception; e != null; e = e.InnerException)
        {
            messages += e.Message + "\n";
        }

        messages.ShouldContain("AbpAgentToolOptions.MaxTools is 1");
    }

    [Fact]
    public void Host_Starts_When_Included_Tools_Fit()
    {
        using var host = new FittingToolsHost();

        host.Catalog.Tools.Count.ShouldBeGreaterThan(1);
    }

    private sealed class TooManyToolsHost : AgentToolsAspNetCoreTestBase
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAgentToolOptions>(o =>
            {
                o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
                o.MaxTools = 1;
            });
        }
    }

    private sealed class FittingToolsHost : AgentToolsAspNetCoreTestBase
    {
        public IAgentToolCatalog Catalog => GetRequiredService<IAgentToolCatalog>();

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAgentToolOptions>(o => o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample));
        }
    }
}
