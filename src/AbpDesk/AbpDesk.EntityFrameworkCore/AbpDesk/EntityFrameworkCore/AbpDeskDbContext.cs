using AbpDesk.Tickets;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace AbpDesk.EntityFrameworkCore
{
    [ConnectionStringName(ConnectionStrings.DefaultConnectionStringName)] //Explicitly declares this module always uses the default connection string
    public class AbpDeskDbContext : AbpDbContext<AbpDeskDbContext>
    {
        public DbSet<Ticket> Tickets { get; set; }

        public AbpDeskDbContext(DbContextOptions<AbpDeskDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Use different classes to map each entity type?
            modelBuilder.Entity<Ticket>(b =>
            {
                b.ToTable("DskTickets");

                b.Property(t => t.Title).HasMaxLength(Ticket.MaxTitleLength).IsRequired();
                b.Property(t => t.Body).HasMaxLength(Ticket.MaxBodyLength);
            });
        }
    }
}
