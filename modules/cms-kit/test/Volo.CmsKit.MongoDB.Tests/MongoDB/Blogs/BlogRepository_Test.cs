using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Blogs;
using Xunit;

namespace Volo.CmsKit.MongoDB.Blogs;

[Collection(MongoTestCollection.Name)]
public class BlogRepository_Test : BlogRepository_Test<CmsKitMongoDbTestModule>
{

}
