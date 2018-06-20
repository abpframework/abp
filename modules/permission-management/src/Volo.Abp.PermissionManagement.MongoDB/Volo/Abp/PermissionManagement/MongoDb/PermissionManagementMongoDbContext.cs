using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    [ConnectionStringName("AbpPermissionManagement")]
    public class PermissionManagementMongoDbContext : AbpMongoDbContext, IPermissionManagementMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = AbpPermissionManagementConsts.DefaultDbTablePrefix;

        public IMongoCollection<PermissionGrant> PermissionGrants => Collection<PermissionGrant>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigurePermissionManagement(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}