using Xunit;

namespace Volo.Abp.AuditLogging.MongoDB;

[Collection(MongoTestCollection.Name)]
public class AuditStore_Basic_Tests : AuditStore_Basic_Tests<AbpAuditLoggingMongoDbTestModule>
{

}
