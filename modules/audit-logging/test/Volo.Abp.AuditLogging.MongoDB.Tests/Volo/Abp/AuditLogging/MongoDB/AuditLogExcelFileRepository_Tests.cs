using Xunit;

namespace Volo.Abp.AuditLogging.MongoDB;

[Collection(MongoTestCollection.Name)]
public class AuditLogExcelFileRepository_Tests : AuditLogExcelFileRepository_Tests<AbpAuditLoggingMongoDbTestModule>
{

}
