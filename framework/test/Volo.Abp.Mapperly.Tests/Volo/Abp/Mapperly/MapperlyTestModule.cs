using Volo.Abp.Mapperly.SampleClasses;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Volo.Abp.Mapperly;

[DependsOn(
    typeof(AbpMapperlyModule),
    typeof(AbpObjectExtendingTestModule)
)]
public class MapperlyTestModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance
                .AddOrUpdateProperty<ExtensibleReverseEntity, string>("Tag")
                .AddOrUpdateProperty<ExtensibleReverseEntity, string>("Secret")
                .AddOrUpdateProperty<ExtensibleReverseDto, string>("Tag")
                .AddOrUpdateProperty<ExtensibleReverseDto, string>("Secret")
                .AddOrUpdateProperty<ExtensibleSeededEntity, string>("Tag")
                .AddOrUpdateProperty<ExtensibleSeededDto, string>("Tag")
                .AddOrUpdateProperty<ExtensibleSeededDto, string>("DtoOnly")
                .AddOrUpdateProperty<ExtensibleNonSeededDto, string>("Tag")
                .AddOrUpdateProperty<ExtensibleNonSeededDto, string>("DtoOnly")
                .AddOrUpdateProperty<ExtensibleNoAttributeDto, string>("Counted", options =>
                {
                    options.DefaultValueFactory = () =>
                    {
                        ExtensibleNoAttributeDto.CountedDefaultValueFactoryCalls++;
                        return null!;
                    };
                });
        });
    }
}
