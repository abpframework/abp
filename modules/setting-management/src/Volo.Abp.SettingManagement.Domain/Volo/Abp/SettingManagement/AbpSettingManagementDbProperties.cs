using Volo.Abp.Data;

namespace Volo.Abp.SettingManagement;

public static class AbpSettingManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpSettingManagement";
}
