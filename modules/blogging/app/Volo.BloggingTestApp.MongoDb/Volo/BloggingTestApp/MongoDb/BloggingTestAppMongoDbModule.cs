using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.BloggingTestApp.MongoDb
{
    [DependsOn(
        typeof(IdentityMongoDbModule),
        typeof(BloggingMongoDbModule),
        typeof(SettingManagementMongoDbModule),
        typeof(PermissionManagementMongoDbModule)
    )]
    public class BloggingTestAppMongoDbModule : AbpModule
    {
    }
}
