namespace Volo.Abp.SettingManagement;

public class SettingDefinitionRecordConsts
{
    public static int MaxNameLength { get; set; } = 128;

    public static int MaxDisplayNameLength { get; set; } = 256;

    public static int MaxDescriptionLength { get; set; } = 512;

    public static int MaxDefaultValueLength { get; set; } = SettingConsts.MaxValueLengthValue;

    public static int MaxProvidersLength { get; set; } = 1024;
}
