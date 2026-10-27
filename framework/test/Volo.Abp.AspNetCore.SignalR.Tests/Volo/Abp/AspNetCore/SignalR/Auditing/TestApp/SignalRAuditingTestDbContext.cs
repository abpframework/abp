using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AspNetCore.SignalR.Auditing.TestApp;

public class SignalRAuditingTestDbContext : AbpDbContext<SignalRAuditingTestDbContext>
{
    public DbSet<AuditedNote> AuditedNotes { get; set; }

    public SignalRAuditingTestDbContext(DbContextOptions<SignalRAuditingTestDbContext> options)
        : base(options)
    {
    }
}
