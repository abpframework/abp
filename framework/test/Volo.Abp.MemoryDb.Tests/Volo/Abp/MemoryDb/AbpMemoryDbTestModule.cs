using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.MemoryDb;
using Volo.Abp.Data;
using Volo.Abp.Autofac;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.MemoryDb.JsonConverters;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.MemoryDb
{
    [DependsOn(
        typeof(TestAppModule),
        typeof(AbpMemoryDbModule),
        typeof(AbpAutofacModule))]
    public class AbpMemoryDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connStr = Guid.NewGuid().ToString();

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connStr;
            });

            context.Services.AddMemoryDbContext<TestAppMemoryDbContext>(options =>
            {
                options.AddDefaultRepositories();
                options.AddRepository<City, CityRepository>();
            });

            Configure<Utf8JsonMemoryDbSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new EntityJsonConverter<EntityWithIntPk, int>());
            });
        }
    }
}
