using System;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BlobStoring.Aws;

public class AwsBlobProviderUploadDecision_Tests
{
    private readonly ExposedAwsBlobProvider _provider = new ExposedAwsBlobProvider();

    [Fact]
    public void Should_Keep_The_Plain_PutObject_Behavior_For_Untransformed_Containers()
    {
        // A non-seekable stream of a container without encryption/pipeline
        // must be uploaded exactly like before
        var args = CreateArgs(new BlobContainerConfiguration(), new NonSeekableStream());

        _provider.RequiresRetrySafeUploadPublic(args).ShouldBeFalse();
    }

    [Fact]
    public void Should_Use_The_Retry_Safe_Upload_For_An_Encrypted_Container()
    {
        var configuration = new BlobContainerConfiguration().UseEncryption("test-passphrase");
        var args = CreateArgs(configuration, new NonSeekableStream());

        _provider.RequiresRetrySafeUploadPublic(args).ShouldBeTrue();
    }

    [Fact]
    public void Should_Use_The_Retry_Safe_Upload_For_A_Container_With_PipelineContributors()
    {
        var configuration = new BlobContainerConfiguration();
        configuration.PipelineContributors.Add<FakePipelineContributor>();
        var args = CreateArgs(configuration, new NonSeekableStream());

        _provider.RequiresRetrySafeUploadPublic(args).ShouldBeTrue();
    }

    [Fact]
    public void Should_Not_Use_The_Retry_Safe_Upload_For_A_Seekable_Stream()
    {
        var configuration = new BlobContainerConfiguration().UseEncryption("test-passphrase");
        var args = CreateArgs(configuration, new MemoryStream());

        _provider.RequiresRetrySafeUploadPublic(args).ShouldBeFalse();
    }

    private static BlobProviderSaveArgs CreateArgs(BlobContainerConfiguration configuration, Stream stream)
    {
        return new BlobProviderSaveArgs("test-container", configuration, "test-blob", stream);
    }

    private sealed class ExposedAwsBlobProvider : AwsBlobProvider
    {
        public ExposedAwsBlobProvider()
            : base(null!, null!, null!)
        {
        }

        public bool RequiresRetrySafeUploadPublic(BlobProviderSaveArgs args)
        {
            return RequiresRetrySafeUpload(args);
        }
    }

    private sealed class FakePipelineContributor : IBlobPipelineContributor
    {
        public Task OnSavingAsync(BlobPipelineContext context)
        {
            return Task.CompletedTask;
        }

        public Task OnGettingAsync(BlobPipelineContext context)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class NonSeekableStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count) => 0;
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
