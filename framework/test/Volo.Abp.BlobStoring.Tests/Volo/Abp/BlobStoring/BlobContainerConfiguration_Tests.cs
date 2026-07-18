using System.Linq;
using Shouldly;
using Volo.Abp.BlobStoring.Fakes;
using Xunit;

namespace Volo.Abp.BlobStoring;

public class BlobContainerConfiguration_Tests
{
    [Fact]
    public void Should_Override_Default_Container_ProviderType_When_Named_Container_Has_Its_Own()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.ProviderType = typeof(FakeBlobProvider1);

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.ProviderType = typeof(FakeBlobProvider2);

        namedConfig.ProviderType.ShouldBe(typeof(FakeBlobProvider2));
        defaultConfig.ProviderType.ShouldBe(typeof(FakeBlobProvider1));
    }

    [Fact]
    public void Should_Inherit_Default_Container_NamingNormalizers_When_Provider_Also_Inherited()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.ProviderType = typeof(FakeBlobProvider1);
        defaultConfig.NamingNormalizers.Add<FakeNamingNormalizer>();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);

        namedConfig.GetEffectiveNamingNormalizers().ShouldContain(typeof(FakeNamingNormalizer));
        namedConfig.NamingNormalizers.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Not_Inherit_NamingNormalizers_When_Named_Container_Has_Its_Own_ProviderType()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.ProviderType = typeof(FakeBlobProvider1);
        defaultConfig.NamingNormalizers.Add<FakeNamingNormalizer>();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.ProviderType = typeof(FakeBlobProvider2);

        namedConfig.GetEffectiveNamingNormalizers().ShouldBeEmpty();
    }

    [Fact]
    public void Should_Override_Default_Container_NamingNormalizers_When_Named_Container_Has_Its_Own()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.NamingNormalizers.Add<FakeNamingNormalizer>();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.NamingNormalizers.Add<AnotherFakeNamingNormalizer>();

        var effective = namedConfig.GetEffectiveNamingNormalizers().ToList();
        effective.ShouldContain(typeof(AnotherFakeNamingNormalizer));
        effective.ShouldNotContain(typeof(FakeNamingNormalizer));
    }

    [Fact]
    public void Should_Compose_Default_And_Local_Pipeline_Contributors_With_Provider_Override()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.UseEncryption();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.ProviderType = typeof(FakeBlobProvider2);
        namedConfig.PipelineContributors.Add<FakeReversingPipelineContributor>();

        namedConfig.GetEffectivePipelineContributors().ShouldBe(new[]
        {
            typeof(BlobEncryptionContributor),
            typeof(FakeReversingPipelineContributor)
        });
    }
}
