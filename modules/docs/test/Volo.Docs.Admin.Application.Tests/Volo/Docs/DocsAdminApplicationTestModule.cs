﻿using Volo.Abp.Modularity;
using Volo.Docs.Admin;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsAdminApplicationModule),
        typeof(DocsDomainTestModule)
    )]
    public class DocsAdminApplicationTestModule : AbpModule
    {

    }
}
