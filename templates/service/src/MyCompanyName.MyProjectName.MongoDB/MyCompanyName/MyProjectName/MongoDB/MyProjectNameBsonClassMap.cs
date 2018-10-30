using Volo.Abp.Threading;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public static class MyProjectNameBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                //Register mappings here. Example:
                //BsonClassMap.RegisterClassMap<Question>(map =>
                //{
                //    map.AutoMap();
                //});
            });
        }
    }
}