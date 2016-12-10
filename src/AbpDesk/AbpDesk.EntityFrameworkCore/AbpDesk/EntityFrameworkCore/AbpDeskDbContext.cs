using AbpDesk.Tickets;
using Microsoft.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    public class AbpDeskDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=AbpDesk;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(builder =>
            {
                builder.ToTable("DskTickets");
            });
        }
    }
}
