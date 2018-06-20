using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Volo.Blog.EntityFrameworkCore
{
    public static class BlogDbContextModelBuilderExtensions
    {
        public static void ConfigureBlog(
            [NotNull] this ModelBuilder builder,
            Action<BlogModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BlogModelBuilderConfigurationOptions();
            optionsAction?.Invoke(options);
        }
    }
}
