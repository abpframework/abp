using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Asserts, lazily while the content is read, that the ambient tenant is the
/// tenant the BLOB operation belongs to.
/// </summary>
public class FakeTenantAssertingPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;

    public FakeTenantAssertingPipelineContributor(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public Task OnSavingAsync(BlobPipelineContext context)
    {
        return Task.CompletedTask;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new TenantAssertingStream(context.BlobStream, _currentTenant, context.TenantId);
        return Task.CompletedTask;
    }

    private sealed class TenantAssertingStream : Stream
    {
        private readonly Stream _inner;
        private readonly ICurrentTenant _currentTenant;
        private readonly Guid? _expectedTenantId;

        public TenantAssertingStream(Stream inner, ICurrentTenant currentTenant, Guid? expectedTenantId)
        {
            _inner = inner;
            _currentTenant = currentTenant;
            _expectedTenantId = expectedTenantId;
        }

        public override bool CanRead
        {
            get
            {
                // Stream.CopyToAsync reads CanRead before the first ReadAsync call
                AssertTenant();
                return true;
            }
        }

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            AssertTenant();
            return _inner.Read(buffer, offset, count);
        }

        private void AssertTenant()
        {
            if (_currentTenant.Id != _expectedTenantId)
            {
                throw new AbpException(
                    $"The lazy transformation ran in the tenant '{_currentTenant.Id?.ToString() ?? "host"}' " +
                    $"instead of the tenant of the BLOB operation ('{_expectedTenantId?.ToString() ?? "host"}')!");
            }
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
}
