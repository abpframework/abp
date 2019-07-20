using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.BloggingTestApp.MongoDB
{
    [DependsOn(
        typeof(AbpIdentityMongoDbModule),
        typeof(BloggingMongoDbModule),
        typeof(AbpSettingManagementMongoDbModule),
        typeof(AbpPermissionManagementMongoDbModule)
    )]
    public class BloggingTestAppMongoDbModule : AbpModule
    {
    }
}
