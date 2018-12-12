using MongoDB.Bson.Serialization;
using Volo.Abp.MongoDB;
using Volo.Abp.Threading;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Users;

namespace Volo.Blogging.MongoDB
{
    public static class AbpBloggingBsonClassMap
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                BsonClassMap.RegisterClassMap<BlogUser>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
                BsonClassMap.RegisterClassMap<Blog>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
                BsonClassMap.RegisterClassMap<Post>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
                BsonClassMap.RegisterClassMap<Comment>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
                BsonClassMap.RegisterClassMap<Tag>(map =>
                {
                    map.AutoMap();
                    map.ConfigureExtraProperties();
                });
            });
        }
    }
}
