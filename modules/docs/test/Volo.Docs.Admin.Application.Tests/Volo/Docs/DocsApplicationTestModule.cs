using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;
using Volo.Docs.Admin;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationModule),
        typeof(DocsDomainTestModule)
    )]
    public class DocsApplicationTestModule : AbpModule
    {

    }

    [DependsOn(
        typeof(DocsAdminApplicationModule),
        typeof(DocsDomainTestModule)
    )]
    public class DocsAdminApplicationTestModule : AbpModule
    {

    }
}
