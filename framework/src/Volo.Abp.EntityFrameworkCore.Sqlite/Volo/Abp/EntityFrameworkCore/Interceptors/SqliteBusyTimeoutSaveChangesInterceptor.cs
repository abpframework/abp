using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Volo.Abp.EntityFrameworkCore.Interceptors;

/// <summary>
/// https://github.com/dotnet/efcore/issues/29514
/// </summary>
public class SqliteBusyTimeoutSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly string _pragmaCommand;

    public SqliteBusyTimeoutSaveChangesInterceptor(int timeoutMilliseconds)
    {
        _pragmaCommand = $"PRAGMA busy_timeout={timeoutMilliseconds};";
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context != null)
        {
            eventData.Context.Database.ExecuteSqlRaw(_pragmaCommand);
        }

        return result;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null)
        {
            await eventData.Context.Database.ExecuteSqlRawAsync(_pragmaCommand, cancellationToken: cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
