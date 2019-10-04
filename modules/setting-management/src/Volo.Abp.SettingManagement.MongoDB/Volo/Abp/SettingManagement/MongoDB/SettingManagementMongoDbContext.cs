using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    [ConnectionStringName(AbpSettingManagementConsts.ConnectionStringName)]
    public class SettingManagementMongoDbContext : AbpMongoDbContext, ISettingManagementMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = AbpSettingManagementConsts.DefaultDbTablePrefix;

        public IMongoCollection<Setting> Settings => Collection<Setting>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureSettingManagement(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}