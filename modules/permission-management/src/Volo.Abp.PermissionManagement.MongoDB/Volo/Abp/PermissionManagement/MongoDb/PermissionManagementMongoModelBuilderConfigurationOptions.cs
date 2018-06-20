using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    public class PermissionManagementMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public PermissionManagementMongoModelBuilderConfigurationOptions([NotNull] string tablePrefix = AbpPermissionManagementConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}