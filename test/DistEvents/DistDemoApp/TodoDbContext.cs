using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace DistDemoApp
{
    public class TodoDbContext : AbpDbContext<TodoDbContext>
    {
        public DbSet<TodoItem> TodoItems { get; set; }

        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>(b =>
            {
                b.Property(x => x.Text).IsRequired().HasMaxLength(128);
            });
        }
    }
}