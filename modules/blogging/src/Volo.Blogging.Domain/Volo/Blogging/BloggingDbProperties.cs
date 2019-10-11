namespace Volo.Blogging
{
    public static class BloggingDbProperties
    {
        /// <summary>
        /// Default value: "Blg".
        /// </summary>
        public static string DbTablePrefix { get; set; } = "Blg";

        /// <summary>
        /// Default value: "null".
        /// </summary>
        public static string DbSchema { get; set; } = null;

        /// <summary>
        /// "Blogging".
        /// </summary>
        public const string ConnectionStringName = "Blogging";
    }
}
