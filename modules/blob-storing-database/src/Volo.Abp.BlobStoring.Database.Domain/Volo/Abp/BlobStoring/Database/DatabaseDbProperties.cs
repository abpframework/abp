namespace Volo.Abp.BlobStoring.Database
{
    public static class DatabaseDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Database";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Database";
    }
}
