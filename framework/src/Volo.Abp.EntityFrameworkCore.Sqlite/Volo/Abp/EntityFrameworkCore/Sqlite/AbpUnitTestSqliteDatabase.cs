using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Volo.Abp.EntityFrameworkCore.Sqlite;

/// <summary>
/// A named shared-cache in-memory SQLite database for unit tests. Each <see cref="DbContext"/> opens its
/// own connection via <see cref="ConnectionString"/> rather than sharing one connection object, so they
/// don't race registering EF's SQLite functions on the same handle. Dispose in OnApplicationShutdown.
/// </summary>
/// <remarks>
/// Shared-cache SQLite has a single writer, so an independent write unit of work (<c>requiresNew</c>)
/// nested inside an open transactional one deadlocks on the second connection. Disable such a side write
/// in the test, or run it against SQL Server.
/// </remarks>
public sealed class AbpUnitTestSqliteDatabase : IDisposable
{
    public string ConnectionString { get; }

    private readonly AbpUnitTestSqliteConnection _keepAliveConnection;

    public AbpUnitTestSqliteDatabase()
    {
        ConnectionString = $"Data Source=AbpUnitTest_{Guid.NewGuid():N};Mode=Memory;Cache=Shared;Pooling=False";
        _keepAliveConnection = new AbpUnitTestSqliteConnection(ConnectionString);
        _keepAliveConnection.Open();
    }

    /// <summary>
    /// Creates the schema for the given contexts (pass contexts built with <see cref="ConnectionString"/>).
    /// The passed contexts are disposed after their tables are created.
    /// </summary>
    public void CreateTables(params DbContext[] dbContexts)
    {
        foreach (var dbContext in dbContexts)
        {
            using (dbContext)
            {
                dbContext.GetService<IRelationalDatabaseCreator>().CreateTables();
            }
        }
    }

    public void Dispose()
    {
        _keepAliveConnection.Dispose();
    }
}
