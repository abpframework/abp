using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Globalization;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class DbNamingConventionRewriterExtensions
    {
        public static void NamingConventionsRewriteName(this DbContextOptionsBuilder optionsBuilder,
            DbNamingConvention namingConvention = DbNamingConvention.Default,
            CultureInfo culture = null)
        {
            switch (namingConvention)
            {
                case DbNamingConvention.Default:
                    break;
                case DbNamingConvention.SnakeCase:
                    optionsBuilder.UseSnakeCaseNamingConvention(culture);
                    break;
                case DbNamingConvention.LowerCase:
                    optionsBuilder.UseLowerCaseNamingConvention(culture);
                    break;
                case DbNamingConvention.UpperCase:
                    optionsBuilder.UseUpperCaseNamingConvention(culture);
                    break;
                case DbNamingConvention.UpperSnakeCase:
                    optionsBuilder.UseUpperSnakeCaseNamingConvention(culture);
                    break;
                default:
                    break;
            }
        }


        public static void NamingConventionsRewriteName(this ModelBuilder modelBuilder,
            DbNamingConvention namingConvention = DbNamingConvention.Default,
            CultureInfo culture = null)
        {
            if (namingConvention == DbNamingConvention.Default)
            {
                return;
            }

            INameRewriter nameRewriter = null;
            culture = culture ?? CultureInfo.InvariantCulture;
            switch (namingConvention)
            {
                case DbNamingConvention.Default:
                    break;
                case DbNamingConvention.SnakeCase:
                    nameRewriter = new SnakeCaseNameRewriter(culture);
                    break;
                case DbNamingConvention.LowerCase:
                    nameRewriter = new LowerCaseNameRewriter(culture);
                    break;
                case DbNamingConvention.UpperCase:
                    nameRewriter = new UpperCaseNameRewriter(culture);
                    break;
                case DbNamingConvention.UpperSnakeCase:
                    nameRewriter = new UpperSnakeCaseNameRewriter(culture);
                    break;
                default:
                    break;
            }

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(nameRewriter.RewriteName(entity.GetTableName()));

                foreach (var property in entity.GetProperties())
                {
                    //property.SetColumnName(nameRewriter.RewriteName(property.GetColumnName()));
                    var columnName = property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName(), null));
                    property.SetColumnName(nameRewriter.RewriteName(columnName));
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(nameRewriter.RewriteName(key.GetName()));
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(nameRewriter.RewriteName(key.GetConstraintName()));
                }

                foreach (var index in entity.GetIndexes())
                {
                    //index.SetName(nameRewriter.RewriteName(index.GetName());
                    index.SetDatabaseName(nameRewriter.RewriteName(index.GetDatabaseName()));
                }
            }

        }

    }
}
