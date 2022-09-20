﻿using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpBackgroundJobsDbProperties.ConnectionStringName)]
public interface IBackgroundJobsMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<BackgroundJobRecord> BackgroundJobs { get; }
}
