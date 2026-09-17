using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Validation;

public class FluentValidationTestAppService_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Should_Validate_With_FluentValidation_On_ConventionalController()
    {
        // Name is "A" which is less than 3 characters, should fail FluentValidation
        var response = await PostAsync("{\"name\": \"A\"}");
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_Validate_With_FluentValidation_On_ConventionalController_EmptyName()
    {
        // Empty name should fail FluentValidation (NotEmpty rule)
        var response = await PostAsync("{\"name\": \"\"}");
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_Validate_With_FluentValidation_On_ConventionalController_MaxLength()
    {
        // Name exceeds 10 characters, should fail FluentValidation (MaximumLength rule)
        var response = await PostAsync("{\"name\": \"12345678901\"}");
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_Return_Validation_Errors_With_Details()
    {
        var response = await PostAsync("{\"name\": \"A\"}");
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();
        content.ShouldContain("Name");
        content.ShouldContain("validationErrors");
    }

    [Fact]
    public async Task Should_Pass_Validation_With_Valid_Input()
    {
        var response = await PostAsync("{\"name\": \"Hello\"}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    private async Task<HttpResponseMessage> PostAsync(string jsonContent)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/app/fluent-validation-test")
        {
            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
        };
        return await Client.SendAsync(request);
    }
}
