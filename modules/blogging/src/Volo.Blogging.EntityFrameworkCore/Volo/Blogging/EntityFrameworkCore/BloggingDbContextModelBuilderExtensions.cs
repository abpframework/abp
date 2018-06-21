using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Volo.Blogging.EntityFrameworkCore
{
    public static class BloggingDbContextModelBuilderExtensions
    {
        public static void ConfigureBlogging(
            [NotNull] this ModelBuilder builder,
            Action<BloggingModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BloggingModelBuilderConfigurationOptions();
            optionsAction?.Invoke(options);
        }
    }
}
