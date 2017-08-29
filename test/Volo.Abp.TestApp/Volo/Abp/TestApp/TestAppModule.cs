using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Repositories;
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
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add((IAbpAutoMapperConfigurationContext ctx) =>
                {
                    ctx.MapperConfiguration.CreateMap<Person, PersonDto>();
                });
            });

            services.AddAssemblyOf<TestAppModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            //TODO: Refactor to a seed class!
            using(IServiceScope scope = context.ServiceProvider.CreateScope())
            {
                var personRepository = scope.ServiceProvider.GetRequiredService<IRepository<Person>>();
                personRepository.Insert(new Person(Guid.NewGuid(), "Douglas", 42));
            }
        }
    }
}
