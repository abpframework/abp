using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Uow;

internal class UnitOfWorkInterceptionScope : IAbpInterceptionScope
{
    private readonly IServiceScope _serviceScope;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    // The unit of work begun by this scope, or null if it initialized the reserved one.
    private readonly IUnitOfWork? _unitOfWork;

    public UnitOfWorkInterceptionScope(IServiceScope serviceScope, IUnitOfWorkManager unitOfWorkManager, IUnitOfWork? unitOfWork)
    {
        _serviceScope = serviceScope;
        _unitOfWorkManager = unitOfWorkManager;
        _unitOfWork = unitOfWork;
    }

    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        if (_unitOfWork != null)
        {
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        else if (_unitOfWorkManager.Current != null)
        {
            await _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }
    }

    public Task FailAsync(Exception exception)
    {
        // Disposing the unit of work without completing it rolls it back; the owner of an outer or reserved one decides for itself.
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        try
        {
            _unitOfWork?.Dispose();
        }
        finally
        {
            _serviceScope.Dispose();
        }
    }
}
