using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.MultiTenancy;

public class CurrentUserTenantResolveContributor_Tests : MultiTenancyTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpTenantResolveOptions>(options =>
        {
            options.TenantResolvers.Add(new TestTenantResolveContributor());
        });
    }

    [Fact]
    public void CurrentUserTenantResolveContributor_Should_Add_First()
    {
        var options = GetRequiredService<IOptions<AbpTenantResolveOptions>>().Value;
        options.TenantResolvers.First().GetType().ShouldBe(typeof(CurrentUserTenantResolveContributor));
    }

    class TestTenantResolveContributor : ITenantResolveContributor
    {
        public string Name { get; }

        public Task ResolveAsync(ITenantResolveContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
