using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.Aws;

public class DefaultAmazonS3ClientFactory_Tests : AbpBlobStoringAwsTestBase
{
    private readonly IAmazonS3ClientFactory _amazonS3ClientFactory;

    public DefaultAmazonS3ClientFactory_Tests()
    {
        _amazonS3ClientFactory = GetRequiredService<IAmazonS3ClientFactory>();
    }

    [Fact]
    public async Task Should_Create_S3Client_With_Custom_ServiceURL()
    {
        // Arrange
        var containerConfiguration = new BlobContainerConfiguration();
        const string serviceUrl = "https://minio.example.com:9000";
        
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration)
        {
            AccessKeyId = "test-access-key",
            SecretAccessKey = "test-secret-key",
            Region = "us-east-1",
            ServiceURL = serviceUrl
        };

        // Act
        using var s3Client = await _amazonS3ClientFactory.GetAmazonS3Client(awsConfiguration);

        // Assert
        s3Client.ShouldNotBeNull();
        s3Client.Config.ServiceURL?.TrimEnd('/').ShouldBe(serviceUrl);
        ((AmazonS3Config)s3Client.Config).ForcePathStyle.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Create_S3Client_Without_Custom_ServiceURL()
    {
        // Arrange
        var containerConfiguration = new BlobContainerConfiguration();
        
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration)
        {
            AccessKeyId = "test-access-key",
            SecretAccessKey = "test-secret-key",
            Region = "us-east-1"
            // ServiceURL not set
        };

        // Act
        using var s3Client = await _amazonS3ClientFactory.GetAmazonS3Client(awsConfiguration);

        // Assert
        s3Client.ShouldNotBeNull();
        s3Client.Config.ServiceURL.ShouldBeNull(); // Should use default AWS S3 service
        ((AmazonS3Config)s3Client.Config).ForcePathStyle.ShouldBeFalse(); // Should be false for AWS S3
    }

    [Fact]
    public async Task Should_Create_S3Client_Without_Region_For_S3Compatible_Services()
    {
        // Arrange
        var containerConfiguration = new BlobContainerConfiguration();
        
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration)
        {
            AccessKeyId = "test-access-key",
            SecretAccessKey = "test-secret-key",
            ServiceURL = "https://minio.example.com:9000"
            // Region not set - should work for S3-compatible services
        };

        // Act
        using var s3Client = await _amazonS3ClientFactory.GetAmazonS3Client(awsConfiguration);

        // Assert
        s3Client.ShouldNotBeNull();
        s3Client.Config.ServiceURL?.TrimEnd('/').ShouldBe("https://minio.example.com:9000");
        ((AmazonS3Config)s3Client.Config).ForcePathStyle.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Set_Checksum_Properties_For_S3Compatible_Services()
    {
        // Arrange
        var containerConfiguration = new BlobContainerConfiguration();
        
        var awsConfiguration = new AwsBlobProviderConfiguration(containerConfiguration)
        {
            AccessKeyId = "test-access-key",
            SecretAccessKey = "test-secret-key",
            ServiceURL = "https://r2.cloudflarestorage.com",
            Region = "auto"
        };

        // Act
        using var s3Client = await _amazonS3ClientFactory.GetAmazonS3Client(awsConfiguration);

        // Assert
        s3Client.ShouldNotBeNull();
        var config = (AmazonS3Config)s3Client.Config;
        config.ServiceURL?.TrimEnd('/').ShouldBe("https://r2.cloudflarestorage.com");
        config.ForcePathStyle.ShouldBeTrue();
        config.RequestChecksumCalculation.ShouldBe(RequestChecksumCalculation.WHEN_REQUIRED);
        config.ResponseChecksumValidation.ShouldBe(ResponseChecksumValidation.WHEN_REQUIRED);
    }
}
