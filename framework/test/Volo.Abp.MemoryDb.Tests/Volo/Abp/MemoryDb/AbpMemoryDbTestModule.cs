using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.MemoryDb;
using Volo.Abp.Data;
using Volo.Abp.Autofac;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.MemoryDb.JsonConverters;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.MemoryDb;

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

        context.Services.AddOptions<Utf8JsonMemoryDbSerializerOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                options.JsonSerializerOptions.Converters.Add(new EntityJsonConverter<EntityWithIntPk, int>());
                options.JsonSerializerOptions.TypeInfoResolver = new AbpDefaultJsonTypeInfoResolver(serviceProvider
                    .GetRequiredService<IOptions<AbpSystemTextJsonSerializerModifiersOptions>>());
            });
    }
}
