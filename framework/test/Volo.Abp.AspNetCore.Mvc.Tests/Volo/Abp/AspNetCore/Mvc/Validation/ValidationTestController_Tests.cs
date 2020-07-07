﻿using System.Net;
using System.Threading.Tasks;
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
            var result = await GetResponseAsStringAsync("/api/validation-test/object-result-action-dynamic-length?value1=hello&value3[0]=53&value3[1]=54");
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
        }
    }
}
