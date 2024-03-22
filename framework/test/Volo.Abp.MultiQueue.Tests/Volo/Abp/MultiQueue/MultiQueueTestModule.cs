using Volo.Abp.Modularity;
using Volo.Abp.MultiQueue.Kafka;
using Volo.Abp.MultiQueue.Options;

namespace Volo.Abp.MultiQueue;

[DependsOn(typeof(AbpTestBaseModule), typeof(AbpMultiQueueModule), typeof(AbpMultiQueueKafkaModule))]
public class MultiQueueTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<QueueOptionsContainer>(options =>
        {
            options.AddOptions(new QueueOptionsWarp
            {
                Key = MultiQueueTestConst.ConfigKey,
                QueueType = "Kafka",
                Options = new KafkaQueueOptions
                {
                    Address = "server.dev.ai-care.top",
                    GroupId = "abp-test",
                }
            });
        });
    }
}