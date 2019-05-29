using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace Volo.Abp.Json
{
    [DependsOn(typeof(TimingModule))]
    public class JsonModule : AbpModule
    {

    }
}
