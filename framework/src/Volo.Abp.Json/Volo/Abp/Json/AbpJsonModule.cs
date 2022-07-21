using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace Volo.Abp.Json;

[DependsOn(typeof(AbpJsonNewtonsoftModule), typeof(AbpJsonSystemTextJsonModule))]
public class AbpJsonModule : AbpModule
{

}
