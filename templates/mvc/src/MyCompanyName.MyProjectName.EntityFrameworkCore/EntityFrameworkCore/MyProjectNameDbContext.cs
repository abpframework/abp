using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class MyProjectNameDbContext : AbpDbContext<MyProjectNameDbContext>
    {
        public MyProjectNameDbContext(DbContextOptions<MyProjectNameDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureMyProjectName();
        }
    }
}
