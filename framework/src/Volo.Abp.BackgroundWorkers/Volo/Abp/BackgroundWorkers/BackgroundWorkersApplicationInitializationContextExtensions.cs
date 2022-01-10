using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers;

public static class BackgroundWorkersApplicationInitializationContextExtensions
{
    public async static Task<ApplicationInitializationContext> AddBackgroundWorkerAsync<TWorker>([NotNull] this ApplicationInitializationContext context)
        where TWorker : IBackgroundWorker
    {
        Check.NotNull(context, nameof(context));

        await context.AddBackgroundWorkerAsync(typeof(TWorker));

        return context;
    }

    public async static Task<ApplicationInitializationContext> AddBackgroundWorkerAsync([NotNull] this ApplicationInitializationContext context, [NotNull] Type workerType)
    {
        Check.NotNull(context, nameof(context));
        Check.NotNull(workerType, nameof(workerType));

        if (!workerType.IsAssignableTo<IBackgroundWorker>())
        {
            throw new AbpException($"Given type ({workerType.AssemblyQualifiedName}) must implement the {typeof(IBackgroundWorker).AssemblyQualifiedName} interface, but it doesn't!");
        }

        await context.ServiceProvider
            .GetRequiredService<IBackgroundWorkerManager>()
            .AddAsync(
                (IBackgroundWorker)context.ServiceProvider.GetRequiredService(workerType)
            );

        return context;
    }
}
