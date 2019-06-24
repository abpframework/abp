using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    [ConnectionStringName(AbpAuditLoggingConsts.ConnectionStringName)]
    public class AuditLoggingMongoDbContext : AbpMongoDbContext, IAuditLoggingMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = AbpAuditLoggingConsts.DefaultDbTablePrefix;

        public IMongoCollection<AuditLog> AuditLogs => Collection<AuditLog>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureAuditLogging(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}
