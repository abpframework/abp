using Volo.CmsKit.Ratings;
using Xunit;

namespace Volo.CmsKit.MongoDB.Ratings
{
    [Collection(MongoTestCollection.Name)]
    public class RatingRepository_Tests : RatingRepository_Tests<CmsKitMongoDbTestModule>
    {
        
    }
}