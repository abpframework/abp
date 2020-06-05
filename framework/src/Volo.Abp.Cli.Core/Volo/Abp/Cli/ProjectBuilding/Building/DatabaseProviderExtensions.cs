namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public static class DatabaseProviderExtensions
    {
        public static string ToProviderName(this DatabaseProvider databaseProvider)
        {
            switch (databaseProvider)
            {
                case DatabaseProvider.EntityFrameworkCore: return "ef";
                case DatabaseProvider.MongoDb: return "mongodb";
                case DatabaseProvider.NotSpecified: return "NotSpecified";
                default: return "NotSpecified";
            }
        }
    }
}