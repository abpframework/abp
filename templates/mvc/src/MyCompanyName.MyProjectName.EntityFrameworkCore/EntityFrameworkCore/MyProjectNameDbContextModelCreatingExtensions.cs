using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public static class MyProjectNameDbContextModelCreatingExtensions
    {
        public static void ConfigureMyProjectName(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            var tablePrefix = MyProjectNameConsts.DefaultDbTablePrefix;
            var schema = MyProjectNameConsts.DefaultDbSchema;

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                //b.ToTable(tablePrefix + "Questions", schema);
                
                //Properties
                //b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Configure relations
                //b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Configure indexes
                //b.HasIndex(q => q.CreationTime);
            });
            */
        }
    }
}