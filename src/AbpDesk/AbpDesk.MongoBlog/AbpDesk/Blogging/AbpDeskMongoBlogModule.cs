using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.EmbeddedFiles;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace AbpDesk.Blogging
{
    //TODO: Make this a plugin
    [DependsOn(typeof(AbpMongoDbModule), typeof(AbpAspNetCoreMvcModule))]
    public class AbpDeskMongoBlogModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDbContext<AbpDeskMongoDbContext>(options =>
            {
                options.WithDefaultRepositories();
            });

            services.Configure<EmbeddedFileOptions>(options =>
            {
                options.Sources.Add(
                    new EmbeddedFileSet(
                        "/Areas/",
                        GetType().GetTypeInfo().Assembly,
                        "AbpDesk.MongoBlog.Areas"
                        )
                    );
            });

            services.AddAssemblyOf<AbpDeskMongoBlogModule>();
        }
    }
}
