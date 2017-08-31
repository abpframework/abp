using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.TestApp.Application;

namespace Volo.Abp.TestApp
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class TestAppModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            ConfigureAutoMapper(services);
            services.AddAssemblyOf<TestAppModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add((IAbpAutoMapperConfigurationContext ctx) =>
                {
                    ctx.MapperConfiguration.CreateMap<Person, PersonDto>().ReverseMap();
                });
            });
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (IServiceScope scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<TestDataBuilder>()
                    .Build();
            }
        }
    }
}
