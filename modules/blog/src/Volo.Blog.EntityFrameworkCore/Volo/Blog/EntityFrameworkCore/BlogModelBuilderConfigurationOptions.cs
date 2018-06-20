using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Blog.EntityFrameworkCore
{
    public class BlogModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
    {
        public BlogModelBuilderConfigurationOptions()
            : base(BlogConsts.DefaultDbTablePrefix, BlogConsts.DefaultDbSchema)
        {
        }
    }
}