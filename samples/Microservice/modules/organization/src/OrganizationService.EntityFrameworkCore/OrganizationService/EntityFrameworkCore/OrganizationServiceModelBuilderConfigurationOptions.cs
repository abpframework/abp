using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrganizationService.EntityFrameworkCore
{
    public class OrganizationServiceModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
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