using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    [ConnectionStringName("AbpSettings")]
    public interface IAbpSettingsDbContext : IEfCoreDbContext
    {
        DbSet<Setting> Settings { get; set; }
    }
}