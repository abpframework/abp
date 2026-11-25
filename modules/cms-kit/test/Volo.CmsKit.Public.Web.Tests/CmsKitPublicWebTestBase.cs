using System;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.AspNetCore.TestBase;

namespace Volo.CmsKit.PublicWebTests;

public abstract class CmsKitPublicWebTestBase : AbpWebApplicationFactoryIntegratedTest<CmsKitPublicWebTestProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseContentRoot(AppContext.BaseDirectory);
    }
}
