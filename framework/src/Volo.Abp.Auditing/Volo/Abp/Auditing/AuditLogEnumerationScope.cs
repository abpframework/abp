using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Uow;

namespace Volo.Abp.Auditing;

internal class AuditLogEnumerationScope : IAbpInterceptionScope
{
    private readonly IServiceScope _serviceScope;
    private readonly AuditLogInfo _auditLog;
    private readonly AuditLogActionInfo? _auditLogAction;
    private readonly IAuditLogSaveHandle? _saveHandle;
    private readonly Func<bool, Task<bool>> _shouldWriteAuditLog;
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public AuditLogEnumerationScope(
        IServiceScope serviceScope,
        AuditLogInfo auditLog,
        AuditLogActionInfo? auditLogAction,
        IAuditLogSaveHandle? saveHandle,
        Func<bool, Task<bool>> shouldWriteAuditLog)
    {
        _serviceScope = serviceScope;
        _auditLog = auditLog;
        _auditLogAction = auditLogAction;
        _saveHandle = saveHandle;
        _shouldWriteAuditLog = shouldWriteAuditLog;
    }

    public Task CompleteAsync(CancellationToken cancellationToken)
    {
        AddAuditLogAction();
        return SaveAsync(_auditLog.Exceptions.Any());
    }

    public Task FailAsync(Exception exception)
    {
        AddAuditLogAction();
        if (!_auditLog.Exceptions.Contains(exception))
        {
            _auditLog.Exceptions.Add(exception);
        }

        return SaveAsync(hasError: true);
    }

    public void Dispose()
    {
        try
        {
            _saveHandle?.Dispose();
        }
        finally
        {
            _serviceScope.Dispose();
        }
    }

    private void AddAuditLogAction()
    {
        _stopwatch.Stop();
        if (_auditLogAction != null)
        {
            _auditLogAction.ExecutionDuration = Convert.ToInt32(_stopwatch.Elapsed.TotalMilliseconds);
            _auditLog.Actions.Add(_auditLogAction);
        }
    }

    private async Task SaveAsync(bool hasError)
    {
        if (_saveHandle == null)
        {
            return;
        }

        if (!await _shouldWriteAuditLog(hasError))
        {
            return;
        }

        var unitOfWorkManager = _serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        if (unitOfWorkManager.Current != null)
        {
            try
            {
                await unitOfWorkManager.Current.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!_auditLog.Exceptions.Contains(ex))
                {
                    _auditLog.Exceptions.Add(ex);
                }
            }
        }

        await _saveHandle.SaveAsync();
    }
}
