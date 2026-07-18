using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring.Fakes;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.BlobStoring;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
    )]
public class AbpBlobStoringTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IBlobProvider>(Substitute.For<FakeBlobProvider1>());
        context.Services.AddSingleton<IBlobProvider>(Substitute.For<FakeBlobProvider2>());

        context.Services.AddSingleton<FakeInMemoryBlobProvider>();
        context.Services.AddSingleton<IBlobProvider>(
            serviceProvider => serviceProvider.GetRequiredService<FakeInMemoryBlobProvider>()
        );

        Configure<AbpSettingOptions>(options =>
        {
            var tenantProviderIndex = options.ValueProviders.IndexOf(typeof(TenantSettingValueProvider));
            options.ValueProviders[tenantProviderIndex] = typeof(FakeTenantPassPhraseSettingValueProvider);
        });

        Configure<AbpBlobStoringEncryptionOptions>(options =>
        {
            options.DefaultPassPhrase = "default-global-passphrase";
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers
                .ConfigureDefault(container =>
                {
                    container.SetConfiguration("TestConfigDefault", "TestValueDefault");
                    container.ProviderType = typeof(FakeBlobProvider1);
                })
                .Configure<TestContainer1>(container =>
                {
                    container.SetConfiguration("TestConfig1", "TestValue1");
                    container.ProviderType = typeof(FakeBlobProvider1);
                })
                .Configure<TestContainer2>(container =>
                {
                    container.SetConfiguration("TestConfig2", "TestValue2");
                    container.ProviderType = typeof(FakeBlobProvider2);
                })
                .Configure<TestContainer3>(container =>
                {
                    container.IsMultiTenant = false;
                })
                .Configure<TestContainer4>(container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.UseEncryption("container4-passphrase");
                })
                .Configure<TestContainer5>(container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.UseEncryption();
                })
                .Configure<TestContainer6>(container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add(typeof(FakeReversingPipelineContributor));
                })
                .Configure<TestContainer7>(container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add(typeof(FakeScopeBoundPipelineContributor));
                });
        });
    }
}
