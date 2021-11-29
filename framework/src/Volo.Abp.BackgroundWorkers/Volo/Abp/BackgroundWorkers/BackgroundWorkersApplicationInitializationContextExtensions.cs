using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers
{
    public static class BackgroundWorkersApplicationInitializationContextExtensions
    {
        public static ApplicationInitializationContext AddBackgroundWorker<TWorker>([NotNull] this ApplicationInitializationContext context)
            where TWorker : IBackgroundWorker
        {
            Check.NotNull(context, nameof(context));

            context.AddBackgroundWorker(typeof(TWorker));

            return context;
        }

        public static ApplicationInitializationContext AddBackgroundWorker([NotNull] this ApplicationInitializationContext context, [NotNull] Type workerType)
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(workerType, nameof(workerType));

            if (!workerType.IsAssignableTo<IBackgroundWorker>())
            {
                throw new AbpException($"Given type ({workerType.AssemblyQualifiedName}) must implement the {typeof(IBackgroundWorker).AssemblyQualifiedName} interface, but it doesn't!");
            }

            context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .Add(
                    (IBackgroundWorker)context.ServiceProvider.GetRequiredService(workerType)
                );

            return context;
        }
    }
}
