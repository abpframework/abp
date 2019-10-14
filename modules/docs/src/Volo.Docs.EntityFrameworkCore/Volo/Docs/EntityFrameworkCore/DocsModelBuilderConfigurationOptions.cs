using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Docs.EntityFrameworkCore
{
    public class DocsModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public DocsModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(tablePrefix, schema)
        {
        }
    }
}