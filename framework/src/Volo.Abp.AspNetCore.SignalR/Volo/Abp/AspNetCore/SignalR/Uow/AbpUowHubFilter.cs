using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.SignalR.Uow;

public class AbpUowHubFilter : IHubFilter
{
    public virtual async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
    {
        object result = null;

        var options = await CreateOptionsAsync(invocationContext);

        var unitOfWorkManager = invocationContext.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        using (var uow = unitOfWorkManager.Begin(options))
        {
            result = await next(invocationContext);
            await uow.CompleteAsync();
        }

        return result;
    }

    private async Task<AbpUnitOfWorkOptions> CreateOptionsAsync(HubInvocationContext invocationContext)
    {
        var options = new AbpUnitOfWorkOptions();

        var defaultOptions = invocationContext.ServiceProvider.GetRequiredService<IOptions<AbpUnitOfWorkDefaultOptions>>().Value;
        var uowHubFilterOptions = invocationContext.ServiceProvider.GetRequiredService<IOptions<AbpUowHubFilterOptions>>().Value;
        options.IsTransactional = defaultOptions.CalculateIsTransactional(
            autoValue: invocationContext.ServiceProvider.GetRequiredService<IUnitOfWorkTransactionBehaviourProvider>().IsTransactional
                       ?? await uowHubFilterOptions.IsTransactional(invocationContext)
        );

        return options;
    }
}
