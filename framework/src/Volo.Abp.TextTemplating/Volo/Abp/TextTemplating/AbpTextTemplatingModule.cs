using System;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Scriban;

namespace Volo.Abp.TextTemplating;

[Obsolete("This module will be removed in the future. Please use AbpTextTemplatingScribanModule or AbpTextTemplatingRazorModule.")]
[DependsOn(typeof(AbpTextTemplatingScribanModule))]
public class AbpTextTemplatingModule : AbpModule
{

}
