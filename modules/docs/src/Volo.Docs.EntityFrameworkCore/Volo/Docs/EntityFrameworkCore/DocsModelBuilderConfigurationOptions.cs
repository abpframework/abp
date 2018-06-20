using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Docs.EntityFrameworkCore
{
    public class DocsModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public DocsModelBuilderConfigurationOptions()
            : base(DocsConsts.DefaultDbTablePrefix, DocsConsts.DefaultDbSchema)
        {
        }
    }
}