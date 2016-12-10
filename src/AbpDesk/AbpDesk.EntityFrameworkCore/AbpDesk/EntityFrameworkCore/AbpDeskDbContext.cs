using AbpDesk.Tickets;
using Microsoft.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    public class AbpDeskDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>(builder =>
            {
                builder.ToTable("DskTickets");
            });
        }
    }
}
