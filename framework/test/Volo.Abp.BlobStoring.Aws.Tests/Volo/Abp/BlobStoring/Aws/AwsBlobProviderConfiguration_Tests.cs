using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.Aws;

public class AwsBlobProviderConfiguration_Tests : AbpBlobStoringAwsTestCommonBase
{
    [Fact]
    public void Should_Set_And_Get_ServiceURL()
    {
        // Arrange
        var containerConfiguration = new BlobContainerConfiguration();
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration);
        const string serviceUrl = "https://minio.example.com:9000";

        // Act
        awsConfiguration.ServiceURL = serviceUrl;

        // Assert
        awsConfiguration.ServiceURL.ShouldBe(serviceUrl);
    }

    [Fact]
    public void Should_Return_Null_When_ServiceURL_Not_Set()
    {
        // Arrange
        var containerConfiguration = new BlobContainerConfiguration();
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration);

        // Act & Assert
        awsConfiguration.ServiceURL.ShouldBeNull();
    }

    [Fact]
    public void Should_Configure_ServiceURL_Using_UseAws_Extension()
    {
        // Arrange
        var containerConfiguration = new BlobContainerConfiguration();
        const string serviceUrl = "https://spaces.digitalocean.com";

        // Act
        containerConfiguration.UseAws(config =>
        {
            config.ServiceURL = serviceUrl;
            config.Region = "us-east-1";
        });

        // Assert
        var awsConfig = containerConfiguration.GetAwsConfiguration();
        awsConfig.ServiceURL.ShouldBe(serviceUrl);
    }

    [Fact]
    public void Should_Default_DisablePayloadSigning_To_False()
    {
        var containerConfiguration = new BlobContainerConfiguration();
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration);

        awsConfiguration.DisablePayloadSigning.ShouldBeFalse();
    }

    [Fact]
    public void Should_Set_And_Get_DisablePayloadSigning()
    {
        var containerConfiguration = new BlobContainerConfiguration();
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration);

        awsConfiguration.DisablePayloadSigning = true;

        awsConfiguration.DisablePayloadSigning.ShouldBeTrue();
    }
}
