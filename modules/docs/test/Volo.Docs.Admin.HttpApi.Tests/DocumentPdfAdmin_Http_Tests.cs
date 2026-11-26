using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Http.Modeling;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Common.Documents;
using Xunit;

namespace Volo.Docs.Admin;

public class DocumentPdfAdmin_Http_Tests : AbpWebApplicationFactoryIntegratedTest<DocsHttpApiTestProgram>
{
    private readonly Guid _projectId = Guid.Parse("35f2e091-24c8-44e5-9165-019c66aa28ca");
    private IDocumentPdfAdminAppService Service => ServiceProvider.GetRequiredService<IDocumentPdfAdminAppService>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseContentRoot(AppContext.BaseDirectory);
    }

    [Fact]
    public async Task Should_Not_Generate_Pdf_On_Cross_Site_Get()
    {
        (await Client.GetAsync("/tests/session/sign-in")).EnsureSuccessStatusCode();
        Client.DefaultRequestHeaders.Referrer = new Uri("https://untrusted.example/");

        var response = await Client.GetAsync($"/api/docs/admin/documents/pdf/generate?projectId={_projectId}&version=unwanted&languageCode=en");

        response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        await Service.DidNotReceive().GeneratePdfAsync(Arg.Any<DocumentPdfGeneratorInput>());
    }

    [Fact]
    public async Task Should_Require_Antiforgery_For_Cookie_Post()
    {
        (await Client.GetAsync("/tests/session/sign-in")).EnsureSuccessStatusCode();

        var response = await Client.PostAsJsonAsync("/api/docs/admin/documents/pdf/generate", CreateInput());

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        await Service.DidNotReceive().GeneratePdfAsync(Arg.Any<DocumentPdfGeneratorInput>());
    }

    [Fact]
    public async Task Should_Accept_Json_With_Valid_Cookie_And_Antiforgery_Token()
    {
        (await Client.GetAsync("/tests/session/sign-in")).EnsureSuccessStatusCode();
        var token = await Client.GetStringAsync("/tests/session/token");
        Client.DefaultRequestHeaders.Add("RequestVerificationToken", token);

        var response = await Client.PostAsJsonAsync("/api/docs/admin/documents/pdf/generate", CreateInput());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await response.Content.ReadAsStringAsync());
        await Service.Received(1).GeneratePdfAsync(Arg.Is<DocumentPdfGeneratorInput>(x => x.ProjectId == _projectId && x.Version == "1.0.0" && x.LanguageCode == "en"));
    }

    [Fact]
    public async Task Should_Accept_Json_From_Non_Browser_Clients_Without_Cookies()
    {
        var response = await Client.PostAsJsonAsync("/api/docs/admin/documents/pdf/generate", CreateInput());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await response.Content.ReadAsStringAsync());
        await Service.Received(1).GeneratePdfAsync(Arg.Is<DocumentPdfGeneratorInput>(x => x.ProjectId == _projectId));
    }

    [Fact]
    public async Task Should_Keep_Static_Client_Metadata_In_Sync_With_Post_Body_Contract()
    {
        var model = await Client.GetFromJsonAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition?includeTypes=true");
        var actual = FindGenerateAction(model);
        actual.HttpMethod.ShouldBe("POST");
        actual.Parameters.Single().BindingSourceId.ShouldBe("Body");
        actual.Parameters.Single().Name.ShouldBe("input");

        var assembly = typeof(DocsAdminHttpApiClientModule).Assembly;
        using var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().Single(x => x.EndsWith("docs-admin-generate-proxy.json")));
        using var reader = new StreamReader(stream!);
        var cached = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(await reader.ReadToEndAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var action = FindGenerateAction(cached);
        action.HttpMethod.ShouldBe(actual.HttpMethod);
        action.Url.ShouldBe(actual.Url);
        action.Parameters.Single().BindingSourceId.ShouldBe(actual.Parameters.Single().BindingSourceId);
        action.Parameters.Single().Name.ShouldBe(actual.Parameters.Single().Name);
        action.Parameters.Single().Type.ShouldBe(actual.Parameters.Single().Type);
    }

    private static ActionApiDescriptionModel FindGenerateAction(ApplicationApiDescriptionModel model)
    {
        return model.Modules.Values.SelectMany(x => x.Controllers.Values)
            .Single(x => x.Type == typeof(DocumentPdfAdminController).FullName)
            .Actions.Values.Single(x => x.Name == nameof(DocumentPdfAdminController.GeneratePdfAsync));
    }

    private DocumentPdfGeneratorInput CreateInput()
    {
        return new DocumentPdfGeneratorInput { ProjectId = _projectId, Version = "1.0.0", LanguageCode = "en" };
    }
}
