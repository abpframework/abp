using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    public static class DatabaseDbContextModelCreatingExtensions
    {
        public static void ConfigureDatabase(
            this ModelBuilder builder,
            Action<DatabaseModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new DatabaseModelBuilderConfigurationOptions(
                DatabaseDbProperties.DbTablePrefix,
                DatabaseDbProperties.DbSchema
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