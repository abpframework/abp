using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;

namespace DistDemoApp
{
    public class TodoDbContext : AbpDbContext<TodoDbContext>, IHasEventOutbox, IHasEventInbox
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoSummary> TodoSummaries { get; set; }
        public DbSet<OutgoingEventRecord> OutgoingEvents { get; set; }
        public DbSet<IncomingEventRecord> IncomingEvents { get; set; }

        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ConfigureEventOutbox();
            modelBuilder.ConfigureEventInbox();

            modelBuilder.Entity<TodoItem>(b =>
            {
                b.Property(x => x.Text).IsRequired().HasMaxLength(128);
            });
        }
    }
}