using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;

namespace Volo.Abp.Identity.MongoDB;

// Registers two predefined tenants, each pointing at its own physical MongoDB database.
// Used by SharedUser tests that need true cross-database isolation (the only place where
// AbpIdentityUserValidator's shared-mode owner lookup and FindSharedUserBy*Async deviate from
// single-DB behavior).
[DependsOn(typeof(AbpIdentityMongoDbTestModule))]
public class AbpIdentitySharedUserSeparateDbMongoDbTestModule : AbpModule
{
    public static readonly Guid TenantAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid TenantBId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Fixed db names so MongoSandbox's embedded mongod does not accumulate one db per test
        // method. Each test method generates unique email / userName so cross-test data does not
        // collide on those fields.
        var tenantAConnection = MongoDbFixture.GetConnectionString("AbpIdentity_SharedSeparateDb_TenantA");
        var tenantBConnection = MongoDbFixture.GetConnectionString("AbpIdentity_SharedSeparateDb_TenantB");

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });

        Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants = new[]
            {
                new TenantConfiguration(TenantAId, "tenant-a")
                {
                    ConnectionStrings = new ConnectionStrings
                    {
                        { ConnectionStrings.DefaultConnectionStringName, tenantAConnection }
                    }
                },
                new TenantConfiguration(TenantBId, "tenant-b")
                {
                    ConnectionStrings = new ConnectionStrings
                    {
                        { ConnectionStrings.DefaultConnectionStringName, tenantBConnection }
                    }
                }
            };
        });
    }
}
