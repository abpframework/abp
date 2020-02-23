using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrganizationService.EntityFrameworkCore
{
    public class OrganizationServiceModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OrganizationServiceModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = OrganizationServiceConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = OrganizationServiceConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}