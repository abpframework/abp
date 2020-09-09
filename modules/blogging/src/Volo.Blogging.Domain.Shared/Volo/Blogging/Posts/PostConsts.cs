namespace Volo.Blogging.Posts
{
    public static class PostConsts
    {
        /// <summary>
        /// Default value: 512
        /// </summary>
        public static int MaxTitleLength { get; set; } = 512;

        /// <summary>
        /// Default value: 64
        /// </summary>
        public static int MaxUrlLength { get; set; } = 64;

        /// <summary>
        /// Default value: 1024 * 1024
        /// </summary>
        public static int MaxContentLength { get; set; } = 1024 * 1024; //1MB

        /// <summary>
        /// Default value: 1000
        /// </summary>
        public static int MaxDescriptionLength { get; set; } = 1000;

        /// <summary>
        /// Default value: 60
        /// </summary>
        public static int MaxTitleLengthToBeSeoFriendly { get; set; } = 60;

        /// <summary>
        /// Default value: 200
        /// </summary>
        public static int MaxSeoFriendlyDescriptionLength { get; set; } = 200;
    }
}
