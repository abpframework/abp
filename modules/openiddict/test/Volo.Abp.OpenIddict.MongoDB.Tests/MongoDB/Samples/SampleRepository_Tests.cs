using Volo.Abp.OpenIddict.Samples;
using Xunit;

namespace Volo.Abp.OpenIddict.MongoDB.Samples;

[Collection(MongoTestCollection.Name)]
public class SampleRepository_Tests : SampleRepository_Tests<OpenIddictMongoDbTestModule>
{
    /* Don't write custom repository tests here, instead write to
     * the base class.
     * One exception can be some specific tests related to MongoDB.
     */
}
