using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// Keeps the service scope and the tenant context of the pipeline contributors
/// until the returned (lazily transforming) stream is disposed: every member the
/// wrappers may run work in — including the disposal of the scope itself — executes
/// in the tenant the BLOB belongs to, not in the ambient tenant of the caller.
/// </summary>
internal sealed class BlobPipelineScopeStream : Stream
{
    private readonly Stream _inner;
    private readonly AsyncServiceScope _scope;
    private readonly ICurrentTenant _currentTenant;
    private readonly Guid? _tenantId;
    private readonly IBlobAuthenticatedEndStream? _authenticatedEndSource;
    private bool _authenticatedEndChecked;
    private bool _faulted;
    private bool _disposed;

    public BlobPipelineScopeStream(
        Stream inner,
        AsyncServiceScope scope,
        ICurrentTenant currentTenant,
        Guid? tenantId,
        IBlobAuthenticatedEndStream? authenticatedEndSource = null)
    {
        _inner = inner;
        _scope = scope;
        _currentTenant = currentTenant;
        _tenantId = tenantId;
        // When encryption is enabled, this is the innermost decrypting stream. Its
        // terminal record is verified when this composed stream reaches EOF, so a
        // contributor that stops before the content ends can not hide a truncation.
        _authenticatedEndSource = authenticatedEndSource;
    }

    public override bool CanRead
    {
        get
        {
            if (_disposed)
            {
                return false;
            }

            using (_currentTenant.Change(_tenantId))
            {
                return _inner.CanRead;
            }
        }
    }

    private void EnsureNotDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    public override bool CanSeek
    {
        get
        {
            if (_disposed)
            {
                return false;
            }

            using (_currentTenant.Change(_tenantId))
            {
                return _inner.CanSeek;
            }
        }
    }

    public override bool CanWrite => false;

    public override bool CanTimeout
    {
        get
        {
            if (_disposed)
            {
                return false;
            }

            using (_currentTenant.Change(_tenantId))
            {
                return _inner.CanTimeout;
            }
        }
    }

    public override int ReadTimeout
    {
        get
        {
            EnsureNotDisposed();
            using (_currentTenant.Change(_tenantId))
            {
                return _inner.ReadTimeout;
            }
        }
        set
        {
            EnsureNotDisposed();
            using (_currentTenant.Change(_tenantId))
            {
                _inner.ReadTimeout = value;
            }
        }
    }

    public override long Length
    {
        get
        {
            EnsureNotDisposed();
            using (_currentTenant.Change(_tenantId))
            {
                return _inner.Length;
            }
        }
    }

    public override long Position
    {
        get
        {
            EnsureNotDisposed();
            using (_currentTenant.Change(_tenantId))
            {
                return _inner.Position;
            }
        }
        set
        {
            EnsureNotDisposed();
            using (_currentTenant.Change(_tenantId))
            {
                _inner.Position = value;
            }
        }
    }

    public override void Flush()
    {
        EnsureNotDisposed();
        using (_currentTenant.Change(_tenantId))
        {
            _inner.Flush();
        }
    }

    public override async Task FlushAsync(CancellationToken cancellationToken)
    {
        EnsureNotDisposed();
        using (_currentTenant.Change(_tenantId))
        {
            await _inner.FlushAsync(cancellationToken);
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        EnsureNotDisposed();
        EnsureNotFaulted();
        using (_currentTenant.Change(_tenantId))
        {
            int read;
            try
            {
                read = _inner.Read(buffer, offset, count);
            }
            catch
            {
                // A failed read faults permanently, so a retry layer can not silently continue
                // from a position where an inner contributor already consumed bytes
                _faulted = true;
                throw;
            }

            VerifyAuthenticatedEndIfNeeded(read, count);
            return read;
        }
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        EnsureNotDisposed();
        EnsureNotFaulted();
        // A token cancelled before any I/O leaves the stream untouched (and healthy for a
        // retry); once a read has started, any failure faults it permanently
        cancellationToken.ThrowIfCancellationRequested();
        using (_currentTenant.Change(_tenantId))
        {
            int read;
            try
            {
#if NETSTANDARD2_0
                read = await _inner.ReadAsync(buffer, offset, count, cancellationToken);
#else
                // Dispatch over the modern overload, so a wrapper that only implements
                // ReadAsync(Memory<byte>) also works for callers of the old overload
                read = await _inner.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
#endif
            }
            catch
            {
                _faulted = true;
                throw;
            }

            await VerifyAuthenticatedEndIfNeededAsync(read, count, cancellationToken);
            return read;
        }
    }

    // Runs when the composed stream reaches EOF on a real (non-zero-count) read. A
    // legitimate partial read (stopping early and disposing) never reaches EOF, so it
    // is not affected. A failed check faults the stream permanently, so a read-retry
    // layer can not swallow the integrity error and later see a normal EOF.
    private void VerifyAuthenticatedEndIfNeeded(int read, int count)
    {
        if (read != 0 || count == 0 || _authenticatedEndChecked || _authenticatedEndSource == null)
        {
            return;
        }

        try
        {
            _authenticatedEndSource.EnsureReadToAuthenticatedEnd();
            _authenticatedEndChecked = true;
        }
        catch
        {
            // The check ran and failed on integrity: mark it done and fault permanently
            _authenticatedEndChecked = true;
            _faulted = true;
            throw;
        }
    }

    private async ValueTask VerifyAuthenticatedEndIfNeededAsync(int read, int count, CancellationToken cancellationToken)
    {
        if (read != 0 || count == 0 || _authenticatedEndChecked || _authenticatedEndSource == null)
        {
            return;
        }

        // A token cancelled before the check runs leaves it un-run and the stream healthy, so
        // a retry with a live token can still verify the end
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            await _authenticatedEndSource.EnsureReadToAuthenticatedEndAsync(cancellationToken);
            _authenticatedEndChecked = true;
        }
        catch (OperationCanceledException)
        {
            // The decrypting stream owns the consumption state and faults itself on a mid-read
            // cancellation; a cancellation it lets through without faulting (for example the
            // token trips in the gap after the check above) leaves it healthy, so the outer
            // must not fault either — a retry with a live token can still verify the end
            throw;
        }
        catch
        {
            // A real integrity failure is permanent, so a read-retry layer can not swallow it
            // and later see a normal EOF
            _authenticatedEndChecked = true;
            _faulted = true;
            throw;
        }
    }

    private void EnsureNotFaulted()
    {
        if (_faulted)
        {
            throw new AbpException("The stream can not be read anymore, because a previous read operation has failed!");
        }
    }

#if !NETSTANDARD2_0
    // Forwarded so a wrapper that only implements the modern overloads is not
    // degraded to the byte[] fallback of the base class
    public override int Read(Span<byte> buffer)
    {
        EnsureNotDisposed();
        EnsureNotFaulted();
        using (_currentTenant.Change(_tenantId))
        {
            int read;
            try
            {
                read = _inner.Read(buffer);
            }
            catch
            {
                _faulted = true;
                throw;
            }

            VerifyAuthenticatedEndIfNeeded(read, buffer.Length);
            return read;
        }
    }

    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();
        EnsureNotFaulted();
        cancellationToken.ThrowIfCancellationRequested();
        using (_currentTenant.Change(_tenantId))
        {
            int read;
            try
            {
                read = await _inner.ReadAsync(buffer, cancellationToken);
            }
            catch
            {
                _faulted = true;
                throw;
            }

            await VerifyAuthenticatedEndIfNeededAsync(read, buffer.Length, cancellationToken);
            return read;
        }
    }
#endif

    public override long Seek(long offset, SeekOrigin origin)
    {
        EnsureNotDisposed();
        using (_currentTenant.Change(_tenantId))
        {
            return _inner.Seek(offset, origin);
        }
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;

            // The tenant context is best-effort: failing to enter it must not skip the
            // resource release (which would leak the provider stream, the scope and the
            // derived key), and a later dispose can not recover it since _disposed is set
            IDisposable? tenantChange = null;
            try
            {
                tenantChange = _currentTenant.Change(_tenantId);
            }
            catch
            {
                // ignored: release the resources below without the tenant context
            }

            try
            {
                DisposeInnerAndScope();
            }
            finally
            {
                tenantChange?.Dispose();
            }
        }

        base.Dispose(disposing);
    }

    private void DisposeInnerAndScope()
    {
        try
        {
#if NETSTANDARD2_0
            // Stream has no DisposeAsync on netstandard2.0, but the inner stream
            // may still implement IAsyncDisposable for its async-only cleanup
            if (_inner is IAsyncDisposable innerAsyncDisposable)
            {
                AsyncHelper.RunSync(() => innerAsyncDisposable.DisposeAsync().AsTask());
            }
            else
            {
                _inner.Dispose();
            }
#else
            // Also covers wrappers that only implement DisposeAsync
            AsyncHelper.RunSync(() => _inner.DisposeAsync().AsTask());
#endif
        }
        catch
        {
            // The stream failure is the root cause; a scope dispose
            // failure on top of it must not replace it
            try
            {
                AsyncHelper.RunSync(() => _scope.DisposeAsync().AsTask());
            }
            catch
            {
                // ignored
            }

            throw;
        }

        // A synchronous scope dispose throws when a scoped service only
        // implements IAsyncDisposable, so always release it asynchronously
        AsyncHelper.RunSync(() => _scope.DisposeAsync().AsTask());
    }

#if !NETSTANDARD2_0
    public override async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            _disposed = true;

            IDisposable? tenantChange = null;
            try
            {
                tenantChange = _currentTenant.Change(_tenantId);
            }
            catch
            {
                // ignored: release the resources below without the tenant context
            }

            try
            {
                await DisposeInnerAndScopeAsync();
            }
            finally
            {
                tenantChange?.Dispose();
            }
        }

        await base.DisposeAsync();
    }

    private async ValueTask DisposeInnerAndScopeAsync()
    {
        try
        {
            await _inner.DisposeAsync();
        }
        catch
        {
            try
            {
                await _scope.DisposeAsync();
            }
            catch
            {
                // ignored: the stream failure is the root cause
            }

            throw;
        }

        await _scope.DisposeAsync();
    }
#endif
}
