#if MongoDB
using System.Reflection.Emit;
using Volo.Abp.AuditLogging.MongoDB;
using Volo.Abp.BlobStoring.Database.MongoDB;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement.MongoDB;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.PermissionManagement.MongoDB;
using Volo.Abp.SettingManagement.MongoDB;
using Volo.Abp.TenantManagement.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Volo.CmsKit.MonbgoDB;

[ConnectionStringName("MongoDBDefault")]
public class UnifiedDbContext : AbpMongoDbContext
{
    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigurePermissionManagement();
        modelBuilder.ConfigureSettingManagement();
        modelBuilder.ConfigureAuditLogging();
        modelBuilder.ConfigureIdentity();
        modelBuilder.ConfigureTenantManagement();
        modelBuilder.ConfigureFeatureManagement();
        modelBuilder.ConfigureCmsKit();
        modelBuilder.ConfigureBlobStoring();
    }
}
#endif
