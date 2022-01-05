using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.SettingManagement.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(AbpSettingManagementDbProperties.ConnectionStringName)]
public interface ISettingManagementMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<Setting> Settings { get; }
}
