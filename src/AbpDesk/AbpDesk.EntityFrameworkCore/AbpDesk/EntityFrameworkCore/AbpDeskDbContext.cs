using AbpDesk.Tickets;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    [DatabaseName(ConnectionStrings.DefaultConnectionStringName)]
    public class AbpDeskDbContext : AbpDbContext<AbpDeskDbContext>
    {
        public DbSet<Ticket> Tickets { get; set; }

        public AbpDeskDbContext(DbContextOptions<AbpDeskDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Use different classes to map each entity type?

            modelBuilder.Entity<Ticket>(builder =>
            {
                builder.ToTable("DskTickets");
            });
        }
    }
}
