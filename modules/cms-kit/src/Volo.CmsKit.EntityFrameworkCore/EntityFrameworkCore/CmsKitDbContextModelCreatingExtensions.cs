using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Volo.CmsKit.EntityFrameworkCore
{
    public static class CmsKitDbContextModelCreatingExtensions
    {
        public static void ConfigureCmsKit(
            this ModelBuilder builder,
            Action<CmsKitModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CmsKitModelBuilderConfigurationOptions(
                CmsKitDbProperties.DbTablePrefix,
                CmsKitDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */
        }
    }
}