using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public class MyProjectNameMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public MyProjectNameMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}