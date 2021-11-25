using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Volo.Abp.AuditLogging.MongoDB;

[Collection(MongoTestCollection.Name)]
public class AuditLogRepository_Tests : AuditLogRepository_Tests<AbpAuditLoggingMongoDbTestModule>
{

}
