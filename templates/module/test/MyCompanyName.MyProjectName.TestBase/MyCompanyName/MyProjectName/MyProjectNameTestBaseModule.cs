using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAuthorizationModule),
        typeof(MyProjectNameDomainModule)
        )]
    public class MyProjectNameTestBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowPermissionChecker();

            context.Services.AddAssemblyOf<MyProjectNameTestBaseModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<MyProjectNameTestDataBuilder>()
                    .Build();
            }
        }
    }
}
