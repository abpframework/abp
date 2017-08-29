using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp
{
    public class TestAppModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<TestAppModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using(IServiceScope scope = context.ServiceProvider.CreateScope())
            {
                var personRepository = scope.ServiceProvider.GetRequiredService<IRepository<Person>>();
                personRepository.Insert(new Person("Douglas", 42));
            }
        }
    }
}
