namespace Volo.Blogging
{
    public static class BloggingDbProperties
    {
        /// <summary>
        /// Default value: "Blg".
        /// </summary>
        public static string DbTablePrefix { get; } = "Blg";

        /// <summary>
        /// Default value: "null".
        /// </summary>
        public static string DbSchema { get; } = null;

        /// <summary>
        /// "Blogging".
        /// </summary>
        public const string ConnectionStringName = "Blogging";
    }
}
