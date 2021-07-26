namespace Volo.Abp.Quartz.Database
{
    public static class QuartzDatabaseDbProperties
    {
        public static string DbTablePrefix { get; set; } = "qrtz_";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Quartz";
    }
}
