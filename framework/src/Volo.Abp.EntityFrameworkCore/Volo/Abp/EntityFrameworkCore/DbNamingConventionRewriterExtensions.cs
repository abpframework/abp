using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Volo.Abp.EntityFrameworkCore
{

    public static class DbNamingConventionRewriterExtensions
    {

        public static void ConfigureNamingConvention<TDbContext>(
            this DbContextOptionsBuilder optionsBuilder,
            NamingConventionOptions options) where TDbContext : DbContext
        {
            if (options == null)
            {
                return;
            }

            var opt = options.GetRewriterOrDefualt(typeof(TDbContext));

            if (opt != null)
            {
                switch (opt.NamingStyle)
                {
                    case NamingConvention.None:
                        break;
                    case NamingConvention.CamelCase:
                        optionsBuilder.UseCamelCaseNamingConvention(opt.Culture);
                        break;
                    case NamingConvention.SnakeCase:
                        optionsBuilder.UseSnakeCaseNamingConvention(opt.Culture);
                        break;
                    case NamingConvention.LowerCase:
                        optionsBuilder.UseLowerCaseNamingConvention(opt.Culture);
                        break;
                    case NamingConvention.UpperCase:
                        optionsBuilder.UseUpperCaseNamingConvention(opt.Culture);
                        break;
                    case NamingConvention.UpperSnakeCase:
                        optionsBuilder.UseUpperSnakeCaseNamingConvention(opt.Culture);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ConfigureNamingConvention<TDbContext>(
            this ModelBuilder modelBuilder,
            NamingConventionOptions options) where TDbContext : DbContext
        {
            if (options == null)
            {
                return;
            }

            var opt = options.GetRewriterOrDefualt(typeof(TDbContext));

            if (opt == null)
            {
                return;
            }

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(opt.Rewriter.RewriteName(entity.GetTableName()));

                foreach (var property in entity.GetProperties())
                {
                    //property.SetColumnName(nameRewriter.RewriteName(property.GetColumnName()));
                    var columnName = property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName(), null));
                    property.SetColumnName(opt.Rewriter.RewriteName(columnName));
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(opt.Rewriter.RewriteName(key.GetName()));
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(opt.Rewriter.RewriteName(key.GetConstraintName()));
                }

                foreach (var index in entity.GetIndexes())
                {
                    //index.SetName(nameRewriter.RewriteName(index.GetName());
                    index.SetDatabaseName(opt.Rewriter.RewriteName(index.GetDatabaseName()));
                }

            }

        }

    }




    public class NamingConventionOptions
    {
        Dictionary<Type, NamingRewriter> rewriters = new Dictionary<Type, NamingRewriter>();

        public NamingRewriter GetRewriterOrDefualt<TDbContext>() where TDbContext : DbContext
        {
            var res = rewriters.TryGetValue(typeof(TDbContext), out NamingRewriter obj) ? obj : null;

            if (res == null)
            {
                res = rewriters.TryGetValue(typeof(DbContext), out obj) ? obj : null;
            }

            return res;
        }

        public NamingRewriter GetRewriterOrDefualt(Type dbContextType)
        {
            var res = rewriters.TryGetValue(dbContextType, out NamingRewriter obj) ? obj : null;

            if (res == null)
            {
                res = rewriters.TryGetValue(typeof(DbContext), out obj) ? obj : null;
            }

            return res;
        }

        public void SetDefault(NamingConvention namingStyle, CultureInfo culture = null)
        {
            if (namingStyle == NamingConvention.None)
            {
                return;
            }

            AddOne(typeof(DbContext), namingStyle, culture);
        }

        public void Specify<TDbContext>(NamingConvention namingStyle, CultureInfo culture = null) where TDbContext : DbContext
        {
            if (namingStyle == NamingConvention.None || typeof(TDbContext).FullName == typeof(DbContext).FullName)
            {
                return;
            }

            AddOne(typeof(TDbContext), namingStyle, culture);
        }

        void AddOne(Type type, NamingConvention namingStyle, CultureInfo culture)
        {
            INameRewriter nameRewriter = null;
            culture ??= CultureInfo.InvariantCulture;
            switch (namingStyle)
            {
                case NamingConvention.None:
                    break;
                case NamingConvention.CamelCase:
                    nameRewriter = new CamelCaseNameRewriter(culture);
                    break;
                case NamingConvention.SnakeCase:
                    nameRewriter = new SnakeCaseNameRewriter(culture);
                    break;
                case NamingConvention.LowerCase:
                    nameRewriter = new LowerCaseNameRewriter(culture);
                    break;
                case NamingConvention.UpperCase:
                    nameRewriter = new UpperCaseNameRewriter(culture);
                    break;
                case NamingConvention.UpperSnakeCase:
                    nameRewriter = new UpperSnakeCaseNameRewriter(culture);
                    break;
                default:
                    break;
            }

            rewriters[type] = new NamingRewriter(namingStyle, culture, nameRewriter);
        }

        public class NamingRewriter
        {
            public NamingConvention NamingStyle { get; private set; } = NamingConvention.None;
            public CultureInfo Culture { get; private set; } = CultureInfo.InvariantCulture;
            public INameRewriter Rewriter { get; private set; }

            public NamingRewriter(
                NamingConvention namingStyle,
                CultureInfo culture,
                INameRewriter rewriter)
            {
                this.NamingStyle = namingStyle;
                this.Culture = culture;
                this.Rewriter = rewriter;
            }

        }

    }


}
