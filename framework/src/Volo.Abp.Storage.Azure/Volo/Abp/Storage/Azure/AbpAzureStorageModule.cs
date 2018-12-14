using Volo.Abp.Modularity;

namespace Volo.Abp.Storage.Azure
{
    [DependsOn(typeof(AbpStorageModule))]
    public class AbpAzureStorageModule : AbpModule
    {
        
    }
}