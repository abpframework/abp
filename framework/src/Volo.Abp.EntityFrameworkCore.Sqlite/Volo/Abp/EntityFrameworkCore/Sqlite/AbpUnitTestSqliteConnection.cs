using System.Threading;
using Microsoft.Data.Sqlite;
using Volo.Abp.Threading;

namespace Volo.Abp.EntityFrameworkCore.Sqlite;

/// <summary>
/// This class is for unit testing purposes.
/// It prevents exceptions in concurrent testing because Sqlite is not thread-safe.
/// </summary>
public class AbpUnitTestSqliteConnection : SqliteConnection
{
    public AbpUnitTestSqliteConnection(string connectionString)
        : base(connectionString)
    {
    }

    public override SqliteCommand CreateCommand()
    {
        return new AbpSqliteCommand
        {
            Connection = this,
            CommandTimeout = DefaultTimeout,
            Transaction = Transaction
        };
    }
}

internal class AbpSqliteCommand : SqliteCommand
{
    private readonly static SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

    public override SqliteConnection? Connection
    {
        get => base.Connection;
        set
        {
            using (Semaphore.Lock())
            {
                base.Connection = value;
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        using (Semaphore.Lock())
        {
            base.Dispose(disposing);
        }
    }
}
