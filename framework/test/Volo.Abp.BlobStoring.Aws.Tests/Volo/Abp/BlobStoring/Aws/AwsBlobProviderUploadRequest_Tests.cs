using System.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.Aws;

public class AwsBlobProviderUploadRequest_Tests
{
    private readonly ExposedAwsBlobProvider _provider = new ExposedAwsBlobProvider();

    [Fact]
    public void Should_Wrap_The_Stream_And_Disable_Auto_Close_For_A_Multipart_Request()
    {
        // The SDK's non-seekable multipart path ignores AutoCloseStream and disposes the
        // input, so the source must be protected by the leave-open wrapper
        var source = new MemoryStream();
        var configuration = new AwsBlobProviderConfiguration(new BlobContainerConfiguration()) { DisablePayloadSigning = true };

        var request = _provider.CreateMultipartUploadRequestPublic("bucket", "key", source, configuration);

        request.InputStream.ShouldBeOfType<LeaveOpenStreamWrapper>();
        request.AutoCloseStream.ShouldBeFalse();
        request.DisablePayloadSigning.ShouldBe(true);
        request.BucketName.ShouldBe("bucket");
        request.Key.ShouldBe("key");
    }

    [Fact]
    public void Should_Keep_The_Source_Open_After_The_Multipart_Input_Is_Disposed()
    {
        var source = new MemoryStream();
        var configuration = new AwsBlobProviderConfiguration(new BlobContainerConfiguration());

        var request = _provider.CreateMultipartUploadRequestPublic("bucket", "key", source, configuration);
        request.InputStream.Dispose(); // the SDK disposes the input on the multipart path

        source.CanRead.ShouldBeTrue(); // the wrapper must have left the source open
    }

    [Fact]
    public void Should_Propagate_Disable_Payload_Signing_And_Keep_Ownership_For_A_Put_Object_Request()
    {
        var source = new MemoryStream();
        var configuration = new AwsBlobProviderConfiguration(new BlobContainerConfiguration()) { DisablePayloadSigning = true };

        var request = _provider.CreatePutObjectRequestPublic("bucket", "key", source, configuration);

        request.InputStream.ShouldBeSameAs(source);
        request.AutoCloseStream.ShouldBeFalse();
        request.DisablePayloadSigning.ShouldBe(true);
    }

    [Fact]
    public void Should_Keep_Disable_Payload_Signing_Off_By_Default_For_A_Put_Object_Request()
    {
        var configuration = new AwsBlobProviderConfiguration(new BlobContainerConfiguration());

        var request = _provider.CreatePutObjectRequestPublic("bucket", "key", new MemoryStream(), configuration);

        request.DisablePayloadSigning.ShouldBe(false);
    }

    private sealed class ExposedAwsBlobProvider : AwsBlobProvider
    {
        public ExposedAwsBlobProvider()
            : base(null!, null!, null!)
        {
        }

        public TransferUtilityUploadRequest CreateMultipartUploadRequestPublic(
            string containerName, string blobName, Stream blobStream, AwsBlobProviderConfiguration configuration)
        {
            return CreateMultipartUploadRequest(containerName, blobName, blobStream, configuration);
        }

        public PutObjectRequest CreatePutObjectRequestPublic(
            string containerName, string blobName, Stream blobStream, AwsBlobProviderConfiguration configuration)
        {
            return CreatePutObjectRequest(containerName, blobName, blobStream, configuration);
        }
    }
}
