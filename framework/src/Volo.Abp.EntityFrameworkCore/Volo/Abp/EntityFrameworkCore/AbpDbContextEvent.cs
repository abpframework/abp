using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore
{
    public class AbpDbContextEvent
    {

        public static void OnConfiguring(string nameOfDbContext, DbContextOptionsBuilder builder)
        {
            builder.NamingConventionsRewriteName(AbpCommonDbProperties.DbNamingConvention);
        }

        public static void OnModelCreating(string nameOfDbContext, ModelBuilder builder)
        {
            builder.NamingConventionsRewriteName(AbpCommonDbProperties.DbNamingConvention);
        }

    }
}
