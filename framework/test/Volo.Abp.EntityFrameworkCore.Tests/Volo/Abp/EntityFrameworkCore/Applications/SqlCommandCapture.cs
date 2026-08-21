using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class SqlCommandCapture : DbCommandInterceptor
{
    private static readonly AsyncLocal<ConcurrentQueue<string>> Commands = new();

    public static IDisposable Begin(out ConcurrentQueue<string> commands)
    {
        commands = new ConcurrentQueue<string>();
        Commands.Value = commands;
        return new DisposeAction(() => Commands.Value = null);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Commands.Value?.Enqueue(command.CommandText);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        Commands.Value?.Enqueue(command.CommandText);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}
