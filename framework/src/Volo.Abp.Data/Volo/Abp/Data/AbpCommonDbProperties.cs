namespace Volo.Abp.Data;

public static class AbpCommonDbProperties
{
    /// <summary>
    /// This table prefix is shared by most of the ABP modules.
    /// You can change it to set table prefix for all modules using this.
    /// 
    /// Default value: "Abp".
    /// </summary>
    public static string DbTablePrefix { get; set; } = "Abp";

    /// <summary>
    /// Default value: null.
    /// </summary>
    public static string DbSchema { get; set; } = null;
}
