using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring.Fakes;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.Modularity;

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

        context.Services.Replace(ServiceDescriptor.Transient<IBlobEncryptionKeyProvider, FakeTenantBlobEncryptionKeyProvider>());

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
                    container.UseEncryption("container6-passphrase", allowLegacyPlainText: true);
                })
                .Configure<TestContainer7>(container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.IsMultiTenant = false;
                    container.UseEncryption("container7-shared-passphrase");
                })
                .Configure<TestContainer8>(container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                })
                .Configure("pipeline-markers", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeAPipelineContributor>();
                    container.PipelineContributors.Add<FakeBPipelineContributor>();
                })
                .Configure("pipeline-encrypted", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.UseEncryption("pipeline-passphrase");
                    container.PipelineContributors.Add<FakeAPipelineContributor>();
                })
                .Configure("pipeline-scoped", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeScopedXorPipelineContributor>();
                })
                .Configure("get-bad-encryption-config", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    // A mis-typed value makes reading the encryption flag throw while getting
                    container.SetConfiguration(BlobEncryptionConfigurationNames.Enabled, "not-a-bool");
                })
                .Configure("pipeline-failing-get", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeFailingGetPipelineContributor>();
                })
                .Configure("pipeline-set-throw-save", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeSetThenThrowPipelineContributor>();
                })
                .Configure("pipeline-partial-get", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeFailingGetPipelineContributor>();
                    container.PipelineContributors.Add<FakeAPipelineContributor>();
                })
                .Configure("pipeline-dispose-throw", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeScopedXorPipelineContributor>();
                    container.PipelineContributors.Add<FakeDisposeThrowingPipelineContributor>();
                })
                .Configure("pipeline-async-scoped", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeAsyncScopedPipelineContributor>();
                })
                .Configure("pipeline-encrypted-earlystop", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.UseEncryption("earlystop-passphrase");
                    container.PipelineContributors.Add<FakeEarlyStopPipelineContributor>();
                })
                .Configure("pipeline-unwrap", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeAPipelineContributor>();
                    container.PipelineContributors.Add<FakeOriginalRestoringPipelineContributor>();
                })
                .Configure("pipeline-async-dispose", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeAsyncDisposePipelineContributor>();
                })
                .Configure("pipeline-modern-async", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.PipelineContributors.Add<FakeModernAsyncPipelineContributor>();
                })
                .Configure("pipeline-shared-tenant", container =>
                {
                    container.ProviderType = typeof(FakeInMemoryBlobProvider);
                    container.IsMultiTenant = false;
                    container.PipelineContributors.Add<FakeTenantAssertingPipelineContributor>();
                    container.PipelineContributors.Add<FakeTenantRecordingScopedPipelineContributor>();
                });
        });
    }
}
