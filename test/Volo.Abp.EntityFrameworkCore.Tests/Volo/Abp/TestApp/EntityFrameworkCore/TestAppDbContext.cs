using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.EntityFrameworkCore
{
    public class TestAppDbContext : AbpDbContext<TestAppDbContext>
    {
        public DbSet<Person> People { get; set; }
        
        public TestAppDbContext(DbContextOptions<TestAppDbContext> options) 
            : base(options)
        {

        }
    }
}
