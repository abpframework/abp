using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public class MyProjectNameMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public MyProjectNameMongoModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = MyProjectNameConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}