using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Reflection;

namespace Volo.Abp.Uow;

public class UnitOfWorkInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UnitOfWorkInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        var method = invocation.Method;
        if (!UnitOfWorkHelper.IsUnitOfWorkMethod(method, out var unitOfWorkAttribute))
        {
            await invocation.ProceedAsync();
            return;
        }

        if (TypeHelper.IsAsyncEnumerable(method.ReturnType))
        {
            await InterceptAsyncEnumerableAsync(invocation, unitOfWorkAttribute);
            return;
        }

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var options = CreateOptions(scope.ServiceProvider, invocation, unitOfWorkAttribute);

            var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            //Trying to begin a reserved UOW by AbpUnitOfWorkMiddleware
            if (unitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, options))
            {
                await invocation.ProceedAsync();

                if (unitOfWorkManager.Current != null)
                {
                    await unitOfWorkManager.Current.SaveChangesAsync();
                }

                return;
            }

            using (var uow = unitOfWorkManager.Begin(options))
            {
                await invocation.ProceedAsync();
                await uow.CompleteAsync();
            }
        }
    }

    private Task InterceptAsyncEnumerableAsync(IAbpMethodInvocation invocation, UnitOfWorkAttribute? unitOfWorkAttribute)
    {
        return invocation.ProceedInScopeAsync(() => BeginScope(invocation, unitOfWorkAttribute));
    }

    private IAbpInterceptionScope BeginScope(IAbpMethodInvocation invocation, UnitOfWorkAttribute? unitOfWorkAttribute)
    {
        var scope = _serviceScopeFactory.CreateScope();
        try
        {
            var options = CreateOptions(scope.ServiceProvider, invocation, unitOfWorkAttribute);
            var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

            //Trying to begin a reserved UOW by AbpUnitOfWorkMiddleware
            var unitOfWork = unitOfWorkManager.TryBeginReserved(UnitOfWork.UnitOfWorkReservationName, options)
                ? null
                : unitOfWorkManager.Begin(options);

            return new UnitOfWorkInterceptionScope(scope, unitOfWorkManager, unitOfWork);
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }

    private AbpUnitOfWorkOptions CreateOptions(IServiceProvider serviceProvider, IAbpMethodInvocation invocation, UnitOfWorkAttribute? unitOfWorkAttribute)
    {
        var options = new AbpUnitOfWorkOptions();

        unitOfWorkAttribute?.SetOptions(options);

        if (unitOfWorkAttribute?.IsTransactional == null)
        {
            var defaultOptions = serviceProvider.GetRequiredService<IOptions<AbpUnitOfWorkDefaultOptions>>().Value;
            options.IsTransactional = defaultOptions.CalculateIsTransactional(
                autoValue: serviceProvider.GetRequiredService<IUnitOfWorkTransactionBehaviourProvider>().IsTransactional
                           ?? !invocation.Method.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase)
            );
        }

        return options;
    }
}
