using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    public class PermissionManagementMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public PermissionManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "")
            : base(tablePrefix)
        {
        }
    }
}