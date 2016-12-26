using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.Data.MultiTenancy
{
    public class MultiTenantConnectionStringResolver_Tests : MultiTenancyTestBase
    {
        private readonly IMultiTenancyManager _multiTenancyManager;
        private readonly IConnectionStringResolver _connectionResolver;

        public MultiTenantConnectionStringResolver_Tests()
        {
            _multiTenancyManager = ServiceProvider.GetRequiredService<IMultiTenancyManager>();

            _connectionResolver = ServiceProvider.GetRequiredService<IConnectionStringResolver>();
            _connectionResolver.ShouldBeOfType<MultiTenantConnectionStringResolver>();
        }

        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = "default-value";
                options.ConnectionStrings["db1"] = "db1-default-value";
                options.ConnectionStrings["tenant1#Default"] = "tenant1-default-value";
                options.ConnectionStrings["tenant1#db1"] = "tenant1-db1-value";
            });
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<ITenantConnectionStringStore, MyTenantConnectionStringStore>());
        }

        [Fact]
        public void All_Tests()
        {
            //No tenant in current context
            _connectionResolver.Resolve().ShouldBe("default-value");
            _connectionResolver.Resolve("db1").ShouldBe("db1-default-value");

            //Overrided connection strings for tenant1
            using (_multiTenancyManager.ChangeTenant(new TenantInfo("tenant1")))
            {
                _connectionResolver.Resolve().ShouldBe("tenant1-default-value");
                _connectionResolver.Resolve("db1").ShouldBe("tenant1-db1-value");
            }

            //No tenant in current context
            _connectionResolver.Resolve().ShouldBe("default-value");
            _connectionResolver.Resolve("db1").ShouldBe("db1-default-value");

            //Undefined connection strings for tenant2
            using (_multiTenancyManager.ChangeTenant(new TenantInfo("tenant2")))
            {
                _connectionResolver.Resolve().ShouldBe("default-value");
                _connectionResolver.Resolve("db1").ShouldBe("db1-default-value");
            }
        }

        public class MyTenantConnectionStringStore : ITenantConnectionStringStore
        {
            private readonly IOptions<DbConnectionOptions> _options;

            public MyTenantConnectionStringStore(IOptions<DbConnectionOptions> options)
            {
                _options = options;
            }

            public string GetConnectionStringOrNull(string tenantId, string connStringName)
            {
                if (connStringName != null)
                {
                    if (_options.Value.ConnectionStrings.ContainsKey(tenantId + "#" + connStringName))
                    {
                        return _options.Value.ConnectionStrings[tenantId + "#" + connStringName];
                    }
                }
                else
                {
                    if (_options.Value.ConnectionStrings.ContainsKey(tenantId + "#Default"))
                    {
                        return _options.Value.ConnectionStrings[tenantId + "#Default"];
                    }
                }

                return null;
            }
        }
    }
}
