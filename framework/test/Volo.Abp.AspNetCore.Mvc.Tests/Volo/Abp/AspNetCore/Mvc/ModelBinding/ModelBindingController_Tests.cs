using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding
{
    public abstract class ModelBindingController_Tests : AspNetCoreMvcTestBase
    {
        protected DateTimeKind DateTimeKind { get; set; }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<AbpClockOptions>(x => x.Kind = DateTimeKind);
        }

        [Fact]
        public async Task DateTimeKind_Test()
        {
            var response = await Client.GetAsync("/api/model-Binding-test/DateTimeKind?input=2010-01-01T00:00:00Z");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            resultAsString.ShouldBe(DateTimeKind.ToString().ToLower());
        }

        [Fact]
        public async Task NullableDateTimeKind_Test()
        {
            var response =
                await Client.GetAsync("/api/model-Binding-test/NullableDateTimeKind?input=2010-01-01T00:00:00Z");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            resultAsString.ShouldBe(DateTimeKind.ToString().ToLower());
        }

        [Fact]
        public async Task DisableDateTimeNormalizationDateTimeKind_Test()
        {
            var response =
                await Client.GetAsync(
                    "/api/model-Binding-test/DisableDateTimeNormalizationDateTimeKind?input=2010-01-01T00:00:00Z");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            //Time parameter(2010-01-01T00:00:00Z) with time zone information, so the default Kind is Local.
            resultAsString.ShouldBe(DateTimeKind.Local.ToString().ToLower());
        }

        [Fact]
        public async Task DisableDateTimeNormalizationNullableDateTimeKind_Test()
        {
            var response =
                await Client.GetAsync(
                    "/api/model-Binding-test/DisableDateTimeNormalizationNullableDateTimeKind?input=2010-01-01T00:00:00Z");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            //Time parameter(2010-01-01T00:00:00Z) with time zone information, so the default Kind is Local.
            resultAsString.ShouldBe(DateTimeKind.Local.ToString().ToLower());
        }

        [Fact]
        public async Task ComplexTypeDateTimeKind_Test()
        {
            var response = await Client.GetAsync("/api/model-Binding-test/ComplexTypeDateTimeKind?" +
                                                 "Time1=2010-01-01T00:00:00Z&" +
                                                 "Time2=2010-01-01T00:00:00Z&" +
                                                 "Time3=2010-01-01T00:00:00Z&" +
                                                 "InnerModel.Time4=2010-01-01T00:00:00Z");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            //Time parameter(2010-01-01T00:00:00Z) with time zone information, so the default Kind is Local.
            resultAsString.ShouldBe(
                $"local_{DateTimeKind.ToString().ToLower()}_{DateTimeKind.ToString().ToLower()}_local");
        }
    }

    public class ModelBindingController_Utc_Tests : ModelBindingController_Tests
    {
        public ModelBindingController_Utc_Tests()
        {
            DateTimeKind = DateTimeKind.Utc;
        }
    }

    public class ModelBindingController_Local_Tests : ModelBindingController_Tests
    {
        public ModelBindingController_Local_Tests()
        {
            DateTimeKind = DateTimeKind.Local;
        }
    }
}