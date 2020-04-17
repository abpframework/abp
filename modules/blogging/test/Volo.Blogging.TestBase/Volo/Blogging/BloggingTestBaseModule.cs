using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule)
        )]
    public class BloggingTestBaseModule : AbpModule
    {

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<BloggingTestDataBuilder>()
                    .Build();
            }
        }
    }
}
