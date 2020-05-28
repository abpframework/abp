using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    public class DatabaseModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public DatabaseModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}