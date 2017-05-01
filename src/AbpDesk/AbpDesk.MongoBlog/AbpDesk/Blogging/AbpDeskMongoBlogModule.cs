using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EmbeddedFiles;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

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
                        "Areas"
                        )
                    );
            });

            services.AddAssemblyOf<AbpDeskMongoBlogModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            CreateSeedData(context);
        }

        private static void CreateSeedData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var logger = context.ServiceProvider.GetRequiredService<ILogger<AbpDeskMongoBlogModule>>();

                logger.LogInformation("Running seed data for mongo blog module...");

                using (var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin())
                {
                    var blogPostRepository = scope.ServiceProvider.GetRequiredService<IQueryableRepository<BlogPost>>();
                    if (blogPostRepository.Any())
                    {
                        logger.LogInformation($"No need to seed database since there are already {blogPostRepository.Count()} blog posts!");
                        return;
                    }

                    var guidGenerator = scope.ServiceProvider.GetRequiredService<IGuidGenerator>();

                    blogPostRepository.Insert(
                        new BlogPost(
                            guidGenerator.Create(),
                            "Demo blog post title one!",
                            "Sample body text for the first blog post"
                        )
                    );

                    blogPostRepository.Insert(
                        new BlogPost(
                            guidGenerator.Create(),
                            "Demo blog post title second!",
                            "Sample body text for the second blog post"
                        )
                        {
                            Comments =
                            {
                            new BlogPostComment("John", "Hi, this is a good post! Thank you :)"),
                            new BlogPostComment("Adam", "You are adam! :s")
                            }
                        }
                    );

                    logger.LogInformation("Inserted two blog post. completing the unit of work...");

                    uow.Complete();

                    logger.LogInformation("Completed!");
                }
            }
        }
    }
}
