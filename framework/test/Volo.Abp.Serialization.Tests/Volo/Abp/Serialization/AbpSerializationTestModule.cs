using Volo.Abp.Modularity;

namespace Volo.Abp.Serialization
{
    [DependsOn(typeof(SerializationModule))]
    public class AbpSerializationTestModule : AbpModule
    {

    }
}