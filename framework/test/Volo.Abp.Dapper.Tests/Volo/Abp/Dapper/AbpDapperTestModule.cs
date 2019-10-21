﻿using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Dapper
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreTestModule),
        typeof(AbpDapperModule),
        typeof(AbpAutofacModule))]
    public class AbpDapperTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}