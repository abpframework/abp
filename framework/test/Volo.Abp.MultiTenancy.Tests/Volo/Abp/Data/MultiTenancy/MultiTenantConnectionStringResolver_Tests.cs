using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.Data.MultiTenancy
{
    public class MultiTenantConnectionStringResolver_Tests : MultiTenancyTestBase
    {
        private readonly Guid _tenant1Id = Guid.NewGuid();
        private readonly Guid _tenant2Id = Guid.NewGuid();

        private readonly IConnectionStringResolver _connectionResolver;
        private readonly ICurrentTenant _currentTenant;

        public MultiTenantConnectionStringResolver_Tests()
        {
            _connectionResolver = ServiceProvider.GetRequiredService<IConnectionStringResolver>();
            _connectionResolver.ShouldBeOfType<MultiTenantConnectionStringResolver>();

            _currentTenant = ServiceProvider.GetRequiredService<ICurrentTenant>();
        }

        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = "default-value";
                options.ConnectionStrings["db1"] = "db1-default-value";
            });

            services.Configure<AbpDefaultTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantConfiguration(_tenant1Id, "tenant1")
                    {
                        ConnectionStrings =
                        {
                            { ConnectionStrings.DefaultConnectionStringName, "tenant1-default-value"},
                            {"db1", "tenant1-db1-value"}
}
                    },
                    new TenantConfiguration(_tenant2Id, "tenant2")
                };
            });
        }

        [Fact]
        public async Task All_Tests()
        {
            //No tenant in current context
            (await _connectionResolver.ResolveAsync()).ShouldBe("default-value");
            (await _connectionResolver.ResolveAsync("db1")).ShouldBe("db1-default-value");

            //Overriden connection strings for tenant1
            using (_currentTenant.Change(_tenant1Id))
            {
                (await _connectionResolver.ResolveAsync()).ShouldBe("tenant1-default-value");
                (await _connectionResolver.ResolveAsync("db1")).ShouldBe("tenant1-db1-value");
            }

            //No tenant in current context
            (await _connectionResolver.ResolveAsync()).ShouldBe("default-value");
            (await _connectionResolver.ResolveAsync("db1")).ShouldBe("db1-default-value");

            //Undefined connection strings for tenant2
            using (_currentTenant.Change(_tenant2Id))
            {
                (await _connectionResolver.ResolveAsync()).ShouldBe("default-value");
                (await _connectionResolver.ResolveAsync("db1")).ShouldBe("db1-default-value");
            }
        }
    }
}
