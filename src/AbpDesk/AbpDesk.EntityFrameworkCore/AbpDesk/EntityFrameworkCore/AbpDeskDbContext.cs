using AbpDesk.Tickets;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    public class AbpDeskDbContext : AbpDbContext<AbpDeskDbContext>
    {
        public DbSet<Ticket> Tickets { get; set; }

        public AbpDeskDbContext(DbContextOptions<AbpDeskDbContext> options) 
            : base(options)
        {

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
