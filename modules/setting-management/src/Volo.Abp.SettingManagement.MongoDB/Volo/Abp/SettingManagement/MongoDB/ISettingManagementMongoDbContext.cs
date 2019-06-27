using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    [ConnectionStringName(AbpSettingManagementConsts.ConnectionStringName)]
    public interface ISettingManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Setting> Settings { get; }
    }
}