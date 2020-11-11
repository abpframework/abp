using AutoFilterer.Swagger.OperationFilters;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoFilterer.Tests.Volo.Abp.AutoFilterer
{
    public class ApplicationService_AutoFilterer_Tests : AbpIntegratedTest<ApplicationService_AutoFilterer_Tests.TestModule>
    {
        [Fact]
        public void Should_Have_SwaggerOperatorFilters()
        {
            var options = ServiceProvider.GetService<SwaggerGenOptions>();

            options.OperationFilterDescriptors.ShouldContain(x => x.Type == typeof(OrderableEnumOperationFilter), "OrderableEnumOperationFilter couldn't found. Make sure the method 'UseAutoFiltererParameters()' is called.");
            options.OperationFilterDescriptors.ShouldContain(x => x.Type == typeof(InnerFilterPropertiesOperationFilter), "OrderableEnumOperationFilter couldn't found. Make sure the method 'UseAutoFiltererParameters()' is called.");
        }
    }
}
