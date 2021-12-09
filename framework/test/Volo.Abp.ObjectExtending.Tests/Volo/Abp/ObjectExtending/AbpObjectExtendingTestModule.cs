using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.TestObjects;
using Volo.Abp.Threading;

namespace Volo.Abp.ObjectExtending;

[DependsOn(
    typeof(AbpObjectExtendingModule),
    typeof(AbpTestBaseModule)
    )]
public class AbpObjectExtendingTestModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance
                .AddOrUpdateProperty<ExtensibleTestPerson, string>("Name")
                .AddOrUpdateProperty<ExtensibleTestPerson, int>("Age")
                .AddOrUpdateProperty<ExtensibleTestPerson, string>("NoPairCheck", options => options.CheckPairDefinitionOnMapping = false)
                .AddOrUpdateProperty<ExtensibleTestPerson, string>("CityName")
                .AddOrUpdateProperty<ExtensibleTestPersonDto, string>("Name")
                .AddOrUpdateProperty<ExtensibleTestPersonDto, int>("ChildCount")
                .AddOrUpdateProperty<ExtensibleTestPersonDto, string>("CityName")
                .AddOrUpdateProperty<ExtensibleTestPersonWithRegularPropertiesDto, string>("Name")
                .AddOrUpdateProperty<ExtensibleTestPersonWithRegularPropertiesDto, int>("Age");
        });
    }
}
