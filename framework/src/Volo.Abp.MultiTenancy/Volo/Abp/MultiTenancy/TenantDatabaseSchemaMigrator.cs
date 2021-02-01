using System.Threading.Tasks;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantDatabaseSchemaMigrator
    {
        /// <summary>
        /// Set Current Tenant before calling this method.
        /// </summary>
        /// <returns></returns>
        Task MigrateAsync();
    }
}
