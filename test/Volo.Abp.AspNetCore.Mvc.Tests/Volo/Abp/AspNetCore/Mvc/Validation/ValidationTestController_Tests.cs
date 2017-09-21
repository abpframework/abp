using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http;
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
    }
}
