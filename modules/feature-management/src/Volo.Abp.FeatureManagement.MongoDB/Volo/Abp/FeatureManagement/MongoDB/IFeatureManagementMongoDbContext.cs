﻿using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    [ConnectionStringName(FeatureManagementConsts.ConnectionStringName)]
    public interface IFeatureManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<FeatureValue> FeatureValues { get; }
    }
}
