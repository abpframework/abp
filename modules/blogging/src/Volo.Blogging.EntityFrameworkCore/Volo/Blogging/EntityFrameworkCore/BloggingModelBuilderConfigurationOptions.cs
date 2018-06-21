using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Blogging.EntityFrameworkCore
{
    public class BloggingModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public BloggingModelBuilderConfigurationOptions()
            : base(BloggingConsts.DefaultDbTablePrefix, BloggingConsts.DefaultDbSchema)
        {
        }
    }
}