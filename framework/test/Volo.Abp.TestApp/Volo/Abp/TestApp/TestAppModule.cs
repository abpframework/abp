using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.TestApp.Application.Dto;
using Volo.Abp.Threading;

namespace Volo.Abp.TestApp
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAutoMapperModule)
        )]
    public class TestAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureAutoMapper();
            ConfigureDistributedEventBus();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add(ctx =>
                {
                    ctx.MapperConfiguration.CreateMap<Person, PersonDto>().ReverseMap();
                    ctx.MapperConfiguration.CreateMap<Phone, PhoneDto>().ReverseMap();
                });

                options.AddMaps<TestAppModule>();
            });
        }

        private void ConfigureDistributedEventBus()
        {
           Configure<AbpDistributedEntityEventOptions>(options =>
           {
               options.AutoEventSelectors.Add<Person>();
               options.EtoMappings.Add<Person, PersonEto>();
           });
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                AsyncHelper.RunSync(() => scope.ServiceProvider
                    .GetRequiredService<TestDataBuilder>()
                    .BuildAsync());
            }
        }
    }
}
