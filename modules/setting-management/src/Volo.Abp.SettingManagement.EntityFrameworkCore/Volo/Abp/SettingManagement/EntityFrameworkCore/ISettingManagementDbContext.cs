using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    [ConnectionStringName("AbpSettingManagement")]
    public interface ISettingManagementDbContext : IEfCoreDbContext
    {
        DbSet<Setting> Settings { get; set; }
    }
}