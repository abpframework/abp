using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.BlobStoring.Fakes;
using Xunit;

namespace Volo.Abp.BlobStoring;

public class BlobContainerPipeline_Tests : AbpBlobStoringTestBase
{
    private readonly IBlobContainerFactory _blobContainerFactory;
    private readonly FakeInMemoryBlobProvider _fakeProvider;

    public BlobContainerPipeline_Tests()
    {
        _blobContainerFactory = GetRequiredService<IBlobContainerFactory>();
        _fakeProvider = GetRequiredService<FakeInMemoryBlobProvider>();
    }

    [Fact]
    public async Task Should_Transform_While_Saving_And_Restore_While_Getting()
    {
        var container = _blobContainerFactory.Create("pipeline-markers");
        var content = "pipeline content".GetBytes();
        using var source = new MemoryStream(content);

        await container.SaveAsync("markers-blob", source);

        // Contributors run in the configuration order while saving: A wraps first,
        // B wraps the result, so the stored form starts with the marker of B
        var rawBytes = _fakeProvider.GetRawBytesOrNull("pipeline-markers", "markers-blob");
        rawBytes.ShouldNotBeNull();
        Encoding.UTF8.GetString(rawBytes, 0, 4).ShouldBe("B>A>");

        source.CanRead.ShouldBeTrue(); // The caller keeps the ownership of the original stream

        (await container.GetAllBytesAsync("markers-blob")).ShouldBe(content);
    }

    [Fact]
    public async Task Should_Run_Contributors_On_The_Plain_Content_When_Encryption_Is_Enabled()
    {
        var container = _blobContainerFactory.Create("pipeline-encrypted");
        var content = "encrypted pipeline content".GetBytes();

        await container.SaveAsync("encrypted-blob", content);

        // The encryption always runs after the contributors, so the stored form is ciphertext
        var rawBytes = _fakeProvider.GetRawBytesOrNull("pipeline-encrypted", "encrypted-blob");
        rawBytes.ShouldNotBeNull();
        Encoding.ASCII.GetString(rawBytes, 0, 4).ShouldBe("ABPE");

        (await container.GetAllBytesAsync("encrypted-blob")).ShouldBe(content);
    }

    [Fact]
    public async Task Should_Keep_The_Contributor_Scope_Alive_Until_The_Returned_Stream_Is_Disposed()
    {
        var container = _blobContainerFactory.Create("pipeline-scoped");
        var content = "scoped pipeline content".GetBytes();

        await container.SaveAsync("scoped-blob", content);

        var stream = await container.GetAsync("scoped-blob");

        // The scoped service is used lazily here, after GetAsync already returned
        using var result = new MemoryStream();
        await stream.CopyToAsync(result);
        result.ToArray().ShouldBe(content);

        var disposedCountBefore = FakeScopedMarkerService.DisposedCount;
        stream.Dispose();
        FakeScopedMarkerService.DisposedCount.ShouldBe(disposedCountBefore + 1);
    }

    [Fact]
    public async Task Should_Dispose_The_Provider_Stream_When_Reading_The_Configuration_Fails()
    {
        var container = _blobContainerFactory.Create("get-bad-encryption-config");

        // Save through a plain (well-configured) container to the same provider key,
        // then read through the mis-configured one so the get fails after the provider
        // stream was obtained
        _fakeProvider.SetRawBytes("get-bad-encryption-config", "config-fail-blob", "content".GetBytes());

        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            await container.GetAsync("config-fail-blob");
        });

        _fakeProvider.LastServedStream.ShouldNotBeNull();
        _fakeProvider.LastServedStream!.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Dispose_The_Provider_Stream_When_A_Get_Contributor_Fails()
    {
        var container = _blobContainerFactory.Create("pipeline-failing-get");

        await container.SaveAsync("failing-blob", "failing content".GetBytes());

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await container.GetAsync("failing-blob");
        });

        _fakeProvider.LastServedStream.ShouldNotBeNull();
        _fakeProvider.LastServedStream!.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Dispose_The_Stream_Of_A_Contributor_That_Fails_After_Replacing_It()
    {
        var container = _blobContainerFactory.Create("pipeline-set-throw-save");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await container.SaveAsync("set-throw-blob", "content".GetBytes());
        });

        FakeSetThenThrowPipelineContributor.LastCreatedStream.ShouldNotBeNull();
        FakeSetThenThrowPipelineContributor.LastCreatedStream!.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Dispose_The_Whole_Chain_When_A_Later_Get_Contributor_Fails()
    {
        // The first contributor already wrapped the provider stream when the second one fails
        var container = _blobContainerFactory.Create("pipeline-partial-get");

        await container.SaveAsync("partial-get-blob", "partial content".GetBytes());

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await container.GetAsync("partial-get-blob");
        });

        _fakeProvider.LastServedStream.ShouldNotBeNull();
        _fakeProvider.LastServedStream!.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Release_The_Remaining_Streams_And_The_Scope_When_A_Dispose_Fails()
    {
        var container = _blobContainerFactory.Create("pipeline-dispose-throw");
        var disposedCountBefore = FakeScopedMarkerService.DisposedCount;

        // The save itself succeeds; the injected failure surfaces from the cleanup
        var exception = await Assert.ThrowsAsync<IOException>(async () =>
        {
            await container.SaveAsync("dispose-throw-blob", "content".GetBytes());
        });

        exception.Message.ShouldContain("Injected dispose failure");
        FakeScopedMarkerService.DisposedCount.ShouldBe(disposedCountBefore + 1); // The scope was still released
    }

    [Fact]
    public async Task Should_Keep_The_Container_Tenant_Context_While_The_Returned_Stream_Is_Read()
    {
        // A tenant reads a shared (IsMultiTenant = false) container: the lazy
        // transformation must still run in the host context of the container
        var container = _blobContainerFactory.Create("pipeline-shared-tenant");
        var currentTenant = GetRequiredService<Volo.Abp.MultiTenancy.ICurrentTenant>();
        var content = "shared tenant content".GetBytes();

        using (currentTenant.Change(Guid.NewGuid()))
        {
            await container.SaveAsync("shared-tenant-blob", content);

            using var stream = await container.GetAsync("shared-tenant-blob");
            using var result = new MemoryStream();
            await stream.CopyToAsync(result); // The wrapper asserts the tenant context here

            result.ToArray().ShouldBe(content);
        }
    }

    [Fact]
    public async Task Should_Not_Degrade_A_Modern_Async_Only_Wrapper_Stream()
    {
        var container = _blobContainerFactory.Create("pipeline-modern-async");
        var content = "modern async content".GetBytes();

        await container.SaveAsync("modern-async-blob", content);

        using var stream = await container.GetAsync("modern-async-blob");
        using var result = new MemoryStream();
        await stream.CopyToAsync(result); // Uses ReadAsync(Memory<byte>) on modern runtimes

        result.ToArray().ShouldBe(content);

        // Callers of the old overload must get the same bridging
        using var oldOverloadStream = await container.GetAsync("modern-async-blob");
        var buffer = new byte[content.Length];
        var totalReadCount = 0;
        while (totalReadCount < buffer.Length)
        {
            var readCount = await oldOverloadStream.ReadAsync(buffer, totalReadCount, buffer.Length - totalReadCount, default);
            if (readCount == 0)
            {
                break;
            }

            totalReadCount += readCount;
        }

        buffer.ShouldBe(content);
    }

    [Fact]
    public async Task Should_Dispose_The_Contributor_Scope_In_The_Container_Tenant_Context()
    {
        var container = _blobContainerFactory.Create("pipeline-shared-tenant");
        var currentTenant = GetRequiredService<Volo.Abp.MultiTenancy.ICurrentTenant>();
        var content = "scope dispose tenant content".GetBytes();

        using (currentTenant.Change(Guid.NewGuid()))
        {
            await container.SaveAsync("scope-dispose-tenant-blob", content, overrideExisting: true);

            var stream = await container.GetAsync("scope-dispose-tenant-blob");
            using (var result = new MemoryStream())
            {
                await stream.CopyToAsync(result);
            }

            FakeTenantRecordingScopedService.Reset();
            stream.Dispose(); // Synchronous dispose from the tenant context

            FakeTenantRecordingScopedService.HasRecordedDispose.ShouldBeTrue();
            FakeTenantRecordingScopedService.LastDisposeTenantId.ShouldBeNull(); // The container is shared (host)

            var asyncStream = await container.GetAsync("scope-dispose-tenant-blob");
            using (var result = new MemoryStream())
            {
                await asyncStream.CopyToAsync(result);
            }

            FakeTenantRecordingScopedService.Reset();
            await asyncStream.DisposeAsync(); // Asynchronous dispose from the tenant context

            FakeTenantRecordingScopedService.HasRecordedDispose.ShouldBeTrue();
            FakeTenantRecordingScopedService.LastDisposeTenantId.ShouldBeNull();
        }
    }

    [Fact]
    public async Task Should_Not_Dispose_The_Original_Stream_When_A_Contributor_Sets_It_Back()
    {
        // A wraps the original, then the second contributor sets the original back:
        // the pipeline must not treat the caller-owned stream as its own
        var container = _blobContainerFactory.Create("pipeline-unwrap");
        var content = "unwrap content".GetBytes();
        using var source = new MemoryStream(content);
        FakeOriginalRestoringPipelineContributor.RestoreTo = source;
        try
        {
            await container.SaveAsync("unwrap-blob", source);
        }
        finally
        {
            FakeOriginalRestoringPipelineContributor.RestoreTo = null;
        }

        source.CanRead.ShouldBeTrue(); // The caller keeps the ownership of the original stream
        _fakeProvider.GetRawBytesOrNull("pipeline-unwrap", "unwrap-blob").ShouldBe(content);
    }

    [Fact]
    public async Task Should_Release_A_Pipeline_Stream_That_Only_Cleans_Up_In_DisposeAsync()
    {
        var container = _blobContainerFactory.Create("pipeline-async-dispose");

        await container.SaveAsync("async-dispose-blob", "async dispose content".GetBytes());

        FakeAsyncDisposePipelineContributor.LastSaveStream.ShouldNotBeNull();
        FakeAsyncDisposePipelineContributor.LastSaveStream!.AsyncDisposed.ShouldBeTrue();

        // The intermediate stream created within the same contributor call is disposed too
        FakeAsyncDisposePipelineContributor.IntermediateSaveStream.ShouldNotBeNull();
        FakeAsyncDisposePipelineContributor.IntermediateSaveStream!.AsyncDisposed.ShouldBeTrue();
    }

    [Fact]
    public void Should_Bridge_A_Synchronous_Dispose_To_The_Async_Cleanup_Of_The_Cipher_Stream()
    {
        // The decrypting stream owns the provider (cipher) stream; a synchronous
        // Dispose of it must still run the async-only cleanup of that stream
        var cipherStream = new AsyncOnlyDisposeStream();
        var decryptingStream = new ChunkedDecryptingReadStream(
            cipherStream, new byte[16], new byte[32], new byte[8], 64 * 1024);

        decryptingStream.Dispose();

        cipherStream.AsyncDisposed.ShouldBeTrue();
    }

    private sealed class AsyncOnlyDisposeStream : Stream
    {
        public bool AsyncDisposed { get; private set; }

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

        // The cleanup only happens asynchronously; a synchronous Dispose does nothing,
        // so the test fails if the decrypting stream does not bridge to DisposeAsync
        public override ValueTask DisposeAsync()
        {
            AsyncDisposed = true;
            return default;
        }
    }

    [Fact]
    public async Task Should_Bridge_A_Synchronous_Dispose_To_The_Async_Cleanup_Of_A_Get_Wrapper()
    {
        var container = _blobContainerFactory.Create("pipeline-async-dispose");
        var content = "sync dispose bridge content".GetBytes();

        await container.SaveAsync("sync-bridge-blob", content, overrideExisting: true);

        using (var stream = await container.GetAsync("sync-bridge-blob"))
        {
            using var result = new MemoryStream();
            await stream.CopyToAsync(result);
            result.ToArray().ShouldBe(content);
        } // The synchronous using must still trigger the async-only cleanup

        FakeAsyncDisposePipelineContributor.LastGetStream.ShouldNotBeNull();
        FakeAsyncDisposePipelineContributor.LastGetStream!.AsyncDisposed.ShouldBeTrue();
        _fakeProvider.LastServedStream.ShouldNotBeNull();
        _fakeProvider.LastServedStream!.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Forward_The_Timeout_Capability_Of_The_Wrapped_Stream()
    {
        var currentTenant = GetRequiredService<Volo.Abp.MultiTenancy.ICurrentTenant>();
        var scope = GetRequiredService<IServiceProvider>().CreateAsyncScope();
        var inner = new TimeoutCapableStream();
        await using var stream = new BlobPipelineScopeStream(inner, scope, currentTenant, null);

        stream.CanTimeout.ShouldBeTrue();
        stream.ReadTimeout = 1234;
        stream.ReadTimeout.ShouldBe(1234);
    }

    private sealed class TimeoutCapableStream : MemoryStream
    {
        private int _readTimeout = -1;

        public override bool CanTimeout => true;

        public override int ReadTimeout
        {
            get => _readTimeout;
            set => _readTimeout = value;
        }
    }

    [Fact]
    public async Task Should_Not_Fault_The_Composed_Stream_When_The_End_Source_Cancels_Without_Faulting()
    {
        // The decrypting stream lets a cancellation before any I/O through while staying
        // healthy (so a retry can still verify). The composed outer stream must mirror that:
        // it must not permanently fault on such a cancellation, or the retry can never verify
        var currentTenant = GetRequiredService<Volo.Abp.MultiTenancy.ICurrentTenant>();
        var scope = GetRequiredService<IServiceProvider>().CreateAsyncScope();
        var endSource = new CancelOnceHealthyAuthenticatedEndStream();
        await using var stream = new BlobPipelineScopeStream(endSource, scope, currentTenant, null, endSource);

        var buffer = new byte[16];

        // The first read reaches EOF and runs the end check, which cancels while staying healthy
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await stream.ReadAsync(buffer, 0, buffer.Length);
        });

        // A retry must still be able to run (and pass) the end check, not hit a faulted stream
        var read = await stream.ReadAsync(buffer, 0, buffer.Length);
        read.ShouldBe(0);
        endSource.EndCheckAttempts.ShouldBe(2);
    }

    private sealed class CancelOnceHealthyAuthenticatedEndStream : Stream, IBlobAuthenticatedEndStream
    {
        public int EndCheckAttempts { get; private set; }

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

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => Task.FromResult(0);

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            => new ValueTask<int>(0);

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public void EnsureReadToAuthenticatedEnd()
        {
        }

        public ValueTask EnsureReadToAuthenticatedEndAsync(CancellationToken cancellationToken = default)
        {
            EndCheckAttempts++;
            if (EndCheckAttempts == 1)
            {
                // Cancelled before any I/O: the source stays healthy, exactly like the real
                // decrypting stream when the token trips just after the outer's own check
                throw new OperationCanceledException();
            }

            return default;
        }
    }

    [Fact]
    public async Task Should_Fault_The_Composed_Stream_When_An_Inner_Read_Fails_After_Consuming()
    {
        // An inner contributor consumes a byte from the stream below it and then fails: the
        // composed stream must fault so a read-retry layer can not silently continue from the
        // consumed position and hand the caller content that is missing that byte
        var currentTenant = GetRequiredService<Volo.Abp.MultiTenancy.ICurrentTenant>();
        var scope = GetRequiredService<IServiceProvider>().CreateAsyncScope();
        var underlying = new MemoryStream(new byte[] { 1, 2, 3, 4 });
        var inner = new ConsumeThenThrowStream(underlying);
        await using var stream = new BlobPipelineScopeStream(inner, scope, currentTenant, null);

        var buffer = new byte[16];
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await stream.ReadAsync(buffer, 0, buffer.Length);
        });

        // The failed read must have faulted the stream permanently
        var retry = await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await stream.ReadAsync(buffer, 0, buffer.Length);
        });
        retry.Message.ShouldContain("a previous read operation has failed");
    }

    private sealed class ConsumeThenThrowStream : Stream
    {
        private readonly Stream _inner;
        private bool _thrown;

        public ConsumeThenThrowStream(Stream inner)
        {
            _inner = inner;
        }

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

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (!_thrown)
            {
                _thrown = true;
                // Consume one byte from the stream below, then fail without handing it up
                _ = _inner.Read(new byte[1], 0, 1);
                throw new OperationCanceledException();
            }

            return _inner.ReadAsync(buffer, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    [Fact]
    public async Task Should_Verify_The_Authenticated_End_Through_A_Contributor_That_Stops_Early()
    {
        var container = _blobContainerFactory.Create("pipeline-encrypted-earlystop");
        var content = new byte[100_000];
        new Random(42).NextBytes(content);

        await container.SaveAsync("earlystop-blob", content, overrideExisting: true);

        // The normal round trip works: reading to the end verifies the terminal record
        (await container.GetAllBytesAsync("earlystop-blob")).ShouldBe(content);

        // Strip the 20-byte terminal record from the stored ciphertext
        var raw = _fakeProvider.GetRawBytesOrNull("pipeline-encrypted-earlystop", "earlystop-blob");
        raw.ShouldNotBeNull();
        var truncated = new byte[raw!.Length - 20];
        Array.Copy(raw, truncated, truncated.Length);
        _fakeProvider.SetRawBytes("pipeline-encrypted-earlystop", "earlystop-blob", truncated);

        // Even though the contributor stops at its own declared length, reading the
        // composed stream to EOF must now fail because the terminal record is gone
        using var stream = await container.GetAsync("earlystop-blob");
        var buffer = new byte[content.Length + 1024];

        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await ReadAllAsync(stream, buffer);
        });

        // The failure must be permanent: reading again must not swallow it and return
        // a normal EOF (which a read-retry layer would treat as a complete read)
        await Assert.ThrowsAsync<AbpException>(async () =>
        {
            await stream.ReadAsync(buffer, 0, buffer.Length);
        });
    }

    private static async Task ReadAllAsync(Stream stream, byte[] buffer)
    {
        int read;
        var offset = 0;
        while ((read = await stream.ReadAsync(buffer, offset, buffer.Length - offset)) > 0)
        {
            offset += read;
        }
    }

    [Fact]
    public void Should_Not_Forward_Reads_After_The_Scope_Stream_Is_Disposed()
    {
        // After dispose the contributor scope is gone; reads must not reach the inner
        // stream (a use-after-scope), and CanRead must be consistent with that
        var currentTenant = GetRequiredService<Volo.Abp.MultiTenancy.ICurrentTenant>();
        var scope = GetRequiredService<IServiceProvider>().CreateAsyncScope();
        var inner = new MemoryStream(new byte[10]);
        var stream = new BlobPipelineScopeStream(inner, scope, currentTenant, null);

        stream.Dispose();

        stream.CanRead.ShouldBeFalse();
        Should.Throw<ObjectDisposedException>(() => stream.Read(new byte[1], 0, 1));
    }

    [Fact]
    public async Task Should_Support_A_Synchronous_Dispose_With_An_Async_Only_Scoped_Service()
    {
        var container = _blobContainerFactory.Create("pipeline-async-scoped");
        var content = "async scoped content".GetBytes();

        await container.SaveAsync("async-scoped-blob", content);

        var stream = await container.GetAsync("async-scoped-blob");
        using var result = new MemoryStream();
        await stream.CopyToAsync(result);
        result.ToArray().ShouldBe(content);

        var asyncDisposedCountBefore = FakeAsyncOnlyDisposableService.AsyncDisposedCount;
        stream.Dispose(); // Must not throw although the scoped service is async-only disposable
        FakeAsyncOnlyDisposableService.AsyncDisposedCount.ShouldBe(asyncDisposedCountBefore + 1);
    }
}
