using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(BackgroundJobsDbProperties.ConnectionStringName)]
public interface IBackgroundJobsDbContext : IEfCoreDbContext
{
    DbSet<BackgroundJobRecord> BackgroundJobs { get; }
}
