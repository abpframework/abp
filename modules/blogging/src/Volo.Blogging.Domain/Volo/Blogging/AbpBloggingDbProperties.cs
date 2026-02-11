using Volo.Abp.Data;

namespace Volo.Blogging
{
    public static class AbpBloggingDbProperties
    {
        /// <summary>
        /// Default value: "Blg".
        /// </summary>
        public static string DbTablePrefix { get; set; } = "Blg";

        /// <summary>
        /// Default value: "null".
        /// </summary>
        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        /// <summary>
        /// "Blogging".
        /// </summary>
        public const string ConnectionStringName = "Blogging";
    }
}
