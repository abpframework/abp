﻿using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.MongoDB
{
    [DependsOn(
        typeof(CmsKitTestBaseModule),
        typeof(CmsKitMongoDbModule)
        )]
    public class CmsKitMongoDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connectionString = MongoDbFixture.ConnectionString.EnsureEndsWith('/') +
                                   "Db_" +
                                    Guid.NewGuid().ToString("N");

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
        }
    }
}