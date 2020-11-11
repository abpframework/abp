using AutoFilterer.Swagger.OperationFilters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoFilterer.Tests.Volo.Abp.AutoFilterer
{
    public class ApplicationService_AutoFilterer_Tests : AbpIntegratedTest<ApplicationService_AutoFilterer_Tests.TestModule>
    {
        [Fact]
        public void Should_Have_SwaggerOperatorFilters()
        {
            var options = ServiceProvider.GetService<IOptions<SwaggerGenOptions>>();

            options.Value.ShouldNotBeNull("Looks like swagger didn't initialized. Check test project.");

            options.Value.OperationFilterDescriptors.ShouldContain(x => x.Type == typeof(OrderableEnumOperationFilter), "OrderableEnumOperationFilter couldn't found. Make sure the method 'UseAutoFiltererParameters()' is called.");
            options.Value.OperationFilterDescriptors.ShouldContain(x => x.Type == typeof(InnerFilterPropertiesOperationFilter), "OrderableEnumOperationFilter couldn't found. Make sure the method 'UseAutoFiltererParameters()' is called.");
        }

        [DependsOn(
            typeof(AbpAutofacModule),
            typeof(AbpSwashbuckleModule),
            typeof(AbpAutoFiltererModule)
            )]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                ConfigureSwaggerServices(context.Services);
            }

            private void ConfigureSwaggerServices(IServiceCollection services)
            {
                services.AddSwaggerGen(
                    options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });
                        options.DocInclusionPredicate((docName, description) => true);
                        options.CustomSchemaIds(type => type.FullName);
                    }
                );
            }
        }
    }
}
