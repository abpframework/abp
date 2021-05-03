using System;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Scriban;

namespace Volo.Abp.TextTemplating
{
    [Obsolete("Use AbpTextTemplatingScribanModule or AbpTextTemplatingRazorModule, This module will remove in the future.")]
    [DependsOn(typeof(AbpTextTemplatingScribanModule))]
    public class AbpTextTemplatingModule : AbpModule
    {

    }
}
