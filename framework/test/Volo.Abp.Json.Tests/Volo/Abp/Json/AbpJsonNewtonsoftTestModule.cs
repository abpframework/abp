using Volo.Abp.Autofac;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace Volo.Abp.Json;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpJsonSystemTextJsonModule),
    typeof(AbpTestBaseModule)
)]
public class AbpJsonSystemTextJsonTestModule : AbpModule
{

}

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpJsonNewtonsoftModule),
    typeof(AbpTestBaseModule)
)]
public class AbpJsonNewtonsoftTestModule : AbpModule
{

}
