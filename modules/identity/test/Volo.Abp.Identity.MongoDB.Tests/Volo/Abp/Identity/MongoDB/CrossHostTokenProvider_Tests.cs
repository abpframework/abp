using System;
using Volo.Abp.Autofac;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Identity.MongoDB;

[Collection(MongoTestCollection.Name)]
public class CrossHostTokenProvider_Tests
    : CrossHostTokenProvider_Tests<MongoCrossHostGeneratorHostModule, MongoCrossHostValidatorHostModule>
{
    protected override Type ValidatorOnlyModuleType => typeof(AbpIdentityAspNetCoreModule);

    protected override IDisposable CreateSharedDatabase()
    {
        CrossHostTokenProviderTestModuleBase.ConnectionString = MongoDbFixture.GetRandomConnectionString();
        return new NullDisposable();
    }

    private sealed class NullDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}

public abstract class MongoCrossHostTestModuleBase : CrossHostTokenProviderTestModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpUnitOfWorkDefaultOptions>(options => options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled);
    }
}

[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityMongoDbModule))]
public class MongoCrossHostGeneratorHostModule : MongoCrossHostTestModuleBase
{
}

[DependsOn(typeof(AbpAutofacModule), typeof(AbpIdentityMongoDbModule), typeof(AbpIdentityAspNetCoreModule))]
public class MongoCrossHostValidatorHostModule : MongoCrossHostTestModuleBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpIdentityAspNetCoreOptions>(options => options.ConfigureAuthentication = false);
    }
}
