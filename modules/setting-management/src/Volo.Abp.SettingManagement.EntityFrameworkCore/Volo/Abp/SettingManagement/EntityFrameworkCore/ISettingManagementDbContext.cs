using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpSettingManagementDbProperties.ConnectionStringName)]
    public interface ISettingManagementDbContext : IEfCoreDbContext
    {
        DbSet<Setting> Settings { get; }
    }
}
