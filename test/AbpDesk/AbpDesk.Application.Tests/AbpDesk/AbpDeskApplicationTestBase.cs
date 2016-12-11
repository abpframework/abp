using AbpDesk.EntityFrameworkCore;
using AbpDesk.Tickets;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.TestBase;

namespace AbpDesk
{
    public abstract class AbpDeskApplicationTestBase : AbpIntegratedTest<AbpDeskApplicationTestModule>
    {
        protected AbpDeskApplicationTestBase()
        {
            SeedTestData();
        }

        protected virtual void SeedTestData()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AbpDeskDbContext>();

                dbContext.Set<Ticket>().Add(new Ticket("My test ticket 1 title", "My test ticket 1 body"));

                dbContext.SaveChanges();
            }
        }
    }
}