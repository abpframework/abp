namespace Volo.Abp.SettingManagement
{
    public static class SettingConsts
    {
        /// <summary>
        /// Default value: 128
        /// </summary>
        public static int MaxNameLength { get; set; } = 128;
        
        /// <summary>
        /// Default value: 2048
        /// </summary>
        public static int MaxValueLength { get; set; } = 2048;
        
        public static int MaxValueLengthValue { get; set; } = MaxValueLength;
        
        /// <summary>
        /// Default value: 64
        /// </summary>
        public static int MaxProviderNameLength { get; set; } = 64;
        
        /// <summary>
        /// Default value: 64
        /// </summary>
        public static int MaxProviderKeyLength { get; set; } = 64;
    }
}
