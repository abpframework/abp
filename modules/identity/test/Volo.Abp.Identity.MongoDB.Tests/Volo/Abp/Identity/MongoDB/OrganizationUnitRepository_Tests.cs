using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Identity.MongoDB
{
    [Collection(MongoTestCollection.Name)]
    public class OrganizationUnitRepository_Tests : OrganizationUnitRepository_Tests<AbpIdentityMongoDbTestModule>
    {
    }
}
