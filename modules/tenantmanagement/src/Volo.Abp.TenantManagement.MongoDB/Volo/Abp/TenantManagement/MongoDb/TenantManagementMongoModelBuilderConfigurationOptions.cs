using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDb
{
    public class TenantManagementMongoModelBuilderConfigurationOptions : MongoModelBuilderConfigurationOptions
    {
        public TenantManagementMongoModelBuilderConfigurationOptions([NotNull] string tablePrefix = AbpTenantManagementConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}