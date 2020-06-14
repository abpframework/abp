namespace Volo.Abp.SettingManagement
{
    public static class SettingConsts
    {
        public const int MaxNameLength = 128;
        public const int MaxValueLength = 2048;
        public static int MaxValueLengthValue { get; set; } = MaxValueLength;
        public const int MaxProviderNameLength = 64;
        public const int MaxProviderKeyLength = 64;
    }
}
