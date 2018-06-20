using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    [ConnectionStringName("AbpSettingManagement")]
    public interface ISettingManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Setting> Settings { get; }
    }
}