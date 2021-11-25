using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering;

[Collection(MongoTestCollection.Name)]
public class HardDelete_Tests : HardDelete_Tests<AbpMongoDbTestModule>
{
}
