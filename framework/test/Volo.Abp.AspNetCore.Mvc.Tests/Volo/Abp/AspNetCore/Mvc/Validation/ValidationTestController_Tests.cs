using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.Http;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    public class ValidationTestController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Should_Validate_Object_Result_Success()
        {
            var result = await GetResponseAsStringAsync("/api/validation-test/object-result-action?value1=hello");
            result.ShouldBe("hello");
        }

        [Fact]
        public async Task Should_Validate_Object_Result_Failing()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action?value1=a", HttpStatusCode.BadRequest); //value1 has min length of 2 chars.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Return_Localized_Validation_Errors()
        {
            using (CultureHelper.Use("tr"))
            {
                var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action?value1=a", HttpStatusCode.BadRequest); //value1 has min length of 2 chars.
                result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);
                result.Error.ValidationErrors[0].Message.ShouldBe("Değer Bir alanı en az 2, en fazla 5 uzunluğunda bir metin olmalıdır.");
            }
        }

        [Fact]
        public async Task Should_Not_Validate_Action_Result_Success()
        {
            var result = await GetResponseAsStringAsync("/api/validation-test/action-result-action?value1=hello");
            result.ShouldBe("ModelState.IsValid: true");
        }

        [Fact]
        public async Task Should_Not_Validate_Action_Result_Failing()
        {
            var result = await GetResponseAsStringAsync("/api/validation-test/action-result-action"); //Missed the value1
            result.ShouldBe("ModelState.IsValid: false");
        }

        [Fact]
        public async Task Should_Return_Custom_Validate_Errors()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>(
                "/api/validation-test/object-result-action-with-custom_validate?value1=abc", HttpStatusCode.BadRequest); //value1 should be hello.

            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);
            result.Error.ValidationErrors.ShouldContain(x => x.Message == "Value1 should be hello");
        }

        [Fact]
        public async Task Should_Validate_Dynamic_Length_Object_Result_Success()
        {
            var result = await GetResponseAsStringAsync("/api/validation-test/object-result-action-dynamic-length?value1=hello&value3[0]=53&value3[1]=54&value4=4&value5=1.5&value6=2004-02-04");
            result.ShouldBe("hello");

        }

        [Fact]
        public async Task Should_Validate_Dynamic_Length_Object_Result_Failing()
        {
            var result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action-dynamic-length?value1=a", HttpStatusCode.BadRequest); //value1 has min string length of 2 chars.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);

            result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action-dynamic-length?value1=12345678", HttpStatusCode.BadRequest); //value1 has max string length of 7 chars.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);

            result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action-dynamic-length?value1=123458&value2=12345", HttpStatusCode.BadRequest); //value2 has max length of 5 chars.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);

            result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action-dynamic-length?value1=123458&value3[0]=53&value3[1]=54&value3[2]=55&value3[3]=56", HttpStatusCode.BadRequest); //value3 has max length of 2.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);

            result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action-dynamic-length?value1=123458&value3[0]=53&value3[1]=54&value[4]=10", HttpStatusCode.BadRequest); //value4 has max num of 5.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);

            result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action-dynamic-length?value1=123458&value3[0]=53&value3[1]=54&value4=2&value5=1.1", HttpStatusCode.BadRequest); //value4 has min num of 1.2.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);

            result = await GetResponseAsObjectAsync<RemoteServiceErrorResponse>("/api/validation-test/object-result-action-dynamic-length?value1=123458&value3[0]=53&value3[1]=54&value4=2&value5=1.2&value6=2004-05-04", HttpStatusCode.BadRequest); //value4 has max date of 3/4/2004.
            result.Error.ValidationErrors.Length.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Disable_Validate()
        {
            var result = await GetResponseAsStringAsync("/api/validation-test/disable-validation-object-result-action");
            result.ShouldBe("ModelState.IsValid: false");
        }

        [Fact] public async Task SubClass_Should_Disable_Validate_If_Class_Has_DisableValidationAttribute()
        {
            var result = await GetResponseAsStringAsync("/api/sub1-validation-test/disable-validation-object-result-action");
            result.ShouldBe("ModelState.IsValid: false");
        }

        [Fact]
        public async Task SubClass_Should_Disable_Validate_If_Action_Has_DisableValidationAttribute()
        {
            var result = await GetResponseAsStringAsync("/api/validation-test/object-result-action2");
            result.ShouldBe("ModelState.IsValid: false");
        }
    }

    public class DisableAutoModelValidationTestController_Tests : AspNetCoreMvcTestBase
    {
        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.AutoModelValidation = false;
            });

            base.ConfigureServices(context, services);
        }

        [Fact]
        public async Task Should_Disable_Validate_If_AutoModelValidation_Is_False()
        {
            var result = await GetResponseAsStringAsync("/api/validation-test/object-result-action3");
            result.ShouldBe("ModelState.IsValid: false");
        }
    }
}
