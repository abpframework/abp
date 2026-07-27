using System;
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
    public void Should_Inherit_Encryption_From_Default_Container()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.UseEncryption();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);

        BlobEncryptionConfiguration.IsEnabled(namedConfig).ShouldBeTrue();
        BlobEncryptionConfiguration.IsEnabled(defaultConfig).ShouldBeTrue();
    }

    [Fact]
    public void Should_Disable_Inherited_Encryption_For_A_Single_Container()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.UseEncryption();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.DisableEncryption();

        BlobEncryptionConfiguration.IsEnabled(namedConfig).ShouldBeFalse();
        BlobEncryptionConfiguration.IsEnabled(defaultConfig).ShouldBeTrue();
    }

    [Fact]
    public void Should_Enable_Encryption_Again_After_Disabling()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.UseEncryption();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.DisableEncryption();
        namedConfig.UseEncryption("named-passphrase");

        BlobEncryptionConfiguration.IsEnabled(namedConfig).ShouldBeTrue();
        BlobEncryptionConfiguration.GetPassPhraseOrNull(namedConfig).ShouldBe("named-passphrase");
    }

    [Fact]
    public void Should_Keep_The_Configured_PassPhrase_When_UseEncryption_Is_Called_Again()
    {
        var configuration = new BlobContainerConfiguration();
        configuration.UseEncryption("first-passphrase", allowLegacyPlainText: true);

        // Another module just ensuring that encryption is enabled must not
        // change the configured key or the legacy option.
        configuration.UseEncryption();

        BlobEncryptionConfiguration.GetPassPhraseOrNull(configuration).ShouldBe("first-passphrase");
        BlobEncryptionConfiguration.IsLegacyPlainTextAllowed(configuration).ShouldBeTrue();
    }

    [Fact]
    public void Should_Shadow_The_PassPhrase_Inherited_From_The_Default_Container()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.UseEncryption("default-passphrase");

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.UseEncryption();
        namedConfig.ClearEncryptionPassPhrase();

        // The named container explicitly opted out of the inherited passphrase
        BlobEncryptionConfiguration.GetPassPhraseOrNull(namedConfig).ShouldBeNull();
        BlobEncryptionConfiguration.GetPassPhraseOrNull(defaultConfig).ShouldBe("default-passphrase");
    }

    [Fact]
    public void Should_Compose_Default_And_Named_Container_PipelineContributors()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.PipelineContributors.Add<FakeAPipelineContributor>();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.PipelineContributors.Add<FakeBPipelineContributor>();

        namedConfig.GetEffectivePipelineContributors()
            .ShouldBe([typeof(FakeAPipelineContributor), typeof(FakeBPipelineContributor)]);
        defaultConfig.GetEffectivePipelineContributors().ShouldBe([typeof(FakeAPipelineContributor)]);
    }

    [Fact]
    public void Should_Keep_Inherited_PipelineContributors_When_The_Provider_Is_Overridden()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.ProviderType = typeof(FakeBlobProvider1);
        defaultConfig.PipelineContributors.Add<FakeAPipelineContributor>();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.ProviderType = typeof(FakeBlobProvider2);

        namedConfig.GetEffectivePipelineContributors().ShouldBe([typeof(FakeAPipelineContributor)]);
    }

    [Fact]
    public void Should_Not_Duplicate_A_PipelineContributor_Configured_On_Both_Levels()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.PipelineContributors.Add<FakeAPipelineContributor>();
        defaultConfig.PipelineContributors.Add<FakeAPipelineContributor>();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.PipelineContributors.Add<FakeAPipelineContributor>();

        // A contributor type runs once, on every configuration level
        defaultConfig.GetEffectivePipelineContributors().ShouldBe([typeof(FakeAPipelineContributor)]);
        namedConfig.GetEffectivePipelineContributors().ShouldBe([typeof(FakeAPipelineContributor)]);
    }

    [Fact]
    public void Should_Opt_Out_Of_The_Inherited_PipelineContributors()
    {
        var defaultConfig = new BlobContainerConfiguration();
        defaultConfig.PipelineContributors.Add<FakeAPipelineContributor>();

        var namedConfig = new BlobContainerConfiguration(defaultConfig);
        namedConfig.InheritPipelineContributors = false;
        namedConfig.PipelineContributors.Add<FakeBPipelineContributor>();

        namedConfig.GetEffectivePipelineContributors().ShouldBe([typeof(FakeBPipelineContributor)]);
    }

    [Fact]
    public void Should_Reject_Empty_PassPhrase()
    {
        var configuration = new BlobContainerConfiguration();

        Assert.ThrowsAny<ArgumentException>(() => configuration.UseEncryption(""));
        Assert.ThrowsAny<ArgumentException>(() => configuration.UseEncryption("   ", allowLegacyPlainText: true));

        // A failed call must not leave the configuration partially modified
        BlobEncryptionConfiguration.IsEnabled(configuration).ShouldBeFalse();
        BlobEncryptionConfiguration.IsLegacyPlainTextAllowed(configuration).ShouldBeFalse();
    }
}
