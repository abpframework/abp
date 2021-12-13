using Volo.Abp.Castle;
using Volo.Abp.Modularity;

namespace Volo.Abp.Autofac;

[DependsOn(typeof(AbpCastleCoreModule))]
public class AbpAutofacModule : AbpModule
{

}
