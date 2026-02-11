using Xunit;

namespace Volo.Abp.AuditLogging.MongoDB;

[Collection(MongoTestCollection.Name)]
public class AuditLogEntityTypeFullNameConverter_Tests : AuditLogEntityTypeFullNameConverter_Tests<AbpAuditLoggingMongoDbTestModule>
{

}
