using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    public interface IAbpSettingsDbContext : IEfCoreDbContext
    {
        DbSet<Setting> Settings { get; set; }
    }
}