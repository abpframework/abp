using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyModuleName.MongoDB
{
    public class MyModuleNameMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public MyModuleNameMongoModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = MyModuleNameConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}