using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Abp.FeatureManagement.EntityFrameworkCore
{
    [ConnectionStringName("FeatureManagement")]
    public interface IFeatureManagementDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}