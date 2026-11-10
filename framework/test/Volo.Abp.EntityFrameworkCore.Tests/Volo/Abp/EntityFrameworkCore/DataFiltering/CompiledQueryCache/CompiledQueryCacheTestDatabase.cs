using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCacheTestDatabase : RelationalDatabase, IDisposable
{
    private readonly CompiledQueryCacheTestDatabaseRecorder _recorder;

    public CompiledQueryCacheTestDatabase(
        DatabaseDependencies dependencies,
        RelationalDatabaseDependencies relationalDependencies,
        CompiledQueryCacheTestDatabaseRecorder recorder)
        : base(dependencies, relationalDependencies)
    {
        _recorder = recorder;
    }

    public override int SaveChanges(IList<IUpdateEntry> entries)
    {
        Interlocked.Increment(ref _recorder.SaveChangesCount);
        return base.SaveChanges(entries);
    }

    public override Task<int> SaveChangesAsync(IList<IUpdateEntry> entries, CancellationToken cancellationToken = default)
    {
        Interlocked.Increment(ref _recorder.SaveChangesCount);
        return base.SaveChangesAsync(entries, cancellationToken);
    }

    public void Dispose()
    {
        Interlocked.Increment(ref _recorder.DisposeCount);
    }
}

public class CompiledQueryCacheTestDatabaseRecorder
{
    public int SaveChangesCount;

    public int DisposeCount;
}
