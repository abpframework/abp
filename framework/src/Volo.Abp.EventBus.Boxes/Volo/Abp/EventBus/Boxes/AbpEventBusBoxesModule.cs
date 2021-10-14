using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Boxes
{
    [DependsOn(
        typeof(AbpEventBusModule),
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpDistributedLockingModule)
        )]
    public class AbpEventBusBoxesModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.AddBackgroundWorker<OutboxSenderManager>();
            context.AddBackgroundWorker<InboxProcessManager>();
        }
    }
}