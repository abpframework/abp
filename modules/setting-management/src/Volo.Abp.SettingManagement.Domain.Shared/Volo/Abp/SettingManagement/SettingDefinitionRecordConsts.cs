namespace Volo.Abp.SettingManagement;

public class SettingDefinitionRecordConsts
{
    public static int MaxNameLength { get; set; } = 128;

    public static int MaxDisplayNameLength { get; set; } = 256;

    public static int MaxDescriptionLength { get; set; } = 512;

    /// <summary>
    /// Default value: 2048
    /// </summary>
    public static int MaxValueLength { get; set; } = 2048;

    public static int MaxDefaultValueLength { get; set; } = MaxValueLength;

    public static int MaxProvidersLength { get; set; } = 1024;
}
