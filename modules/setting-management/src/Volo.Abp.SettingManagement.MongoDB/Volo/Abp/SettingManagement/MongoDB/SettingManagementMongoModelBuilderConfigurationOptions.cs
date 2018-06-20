using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    public class SettingManagementMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public SettingManagementMongoModelBuilderConfigurationOptions([NotNull] string tablePrefix = AbpSettingManagementConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}