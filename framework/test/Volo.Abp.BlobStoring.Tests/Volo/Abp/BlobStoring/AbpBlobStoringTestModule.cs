using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring
{
    [DependsOn(
        typeof(AbpBlobStoringModule),
        typeof(AbpTestBaseModule)
        )]
    public class AbpBlobStoringTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers
                    .Configure<TestContainer1>(container =>
                    {
                        container["TestConfig1"] = "TestValue1";
                    })
                    .Configure<TestContainer2>(container =>
                    {
                        container["TestConfig2"] = "TestValue2";
                    });
            });
        }
    }
}