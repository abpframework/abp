using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.AgentTools.Sample.Diagnostics;
using Volo.Abp.AgentTools.SchemaSample;
using Xunit;

namespace Volo.Abp.AgentTools;

// Schema validation at startup and full-replace update DTOs.
public class AgentToolSchemaValidationTests : AgentToolsAspNetCoreTestBase
{
    [Fact]
    public void Invalid_Schema_Excludes_The_Tool_With_An_Error_Log()
    {
        var logger = new ListLogger<AgentToolCatalog>();
        var catalog = CreateCatalog(o => o.IncludedRemoteServiceNames.Add(DiagnosticsSampleAppService.RemoteServiceName), logger);

        catalog.Tools.Select(t => t.Name).ShouldBe([DiagnosticsSampleAppService.UpdateFull, DiagnosticsSampleAppService.UpdatePatch]);
        catalog.Find(DiagnosticsSampleAppService.CreateWithInvalidPattern).ShouldBeNull();
        catalog.Skipped.ShouldContain(s => s.ActionName == nameof(IDiagnosticsSampleAppService.CreateWithInvalidPatternAsync)
                                           && s.Reason.StartsWith("invalid input schema:") && s.Reason.Contains("[a-z"));
        logger.Entries.ShouldContain(e => e.Level == LogLevel.Error
                                          && e.Message.Contains(DiagnosticsSampleAppService.CreateWithInvalidPattern)
                                          && e.Message.Contains("pattern '[a-z'"));
    }

    [Fact]
    public void FailOnInvalidSchema_Throws_With_The_Tool_And_The_Reason()
    {
        var catalog = CreateCatalog(o =>
        {
            o.IncludedRemoteServiceNames.Add(DiagnosticsSampleAppService.RemoteServiceName);
            o.FailOnInvalidSchema = true;
        });

        var exception = Should.Throw<AbpException>(() => catalog.Tools);

        exception.Message.ShouldContain(DiagnosticsSampleAppService.CreateWithInvalidPattern);
        exception.Message.ShouldContain("is not a valid regular expression");
        exception.Message.ShouldContain("FailOnInvalidSchema");
    }

    [Fact]
    public void Decimal_Range_Produces_A_Valid_Schema()
    {
        var catalog = CreateCatalog(o =>
        {
            o.IncludedRemoteServiceNames.Add(SchemaSampleAppService.RemoteServiceName);
            o.FailOnInvalidSchema = true;
        });

        var echo = catalog.Find(SchemaSampleAppService.GetEcho).ShouldNotBeNull(); // SchemaSampleInput.Price: decimal [Range]
        AgentToolSchemaValidator.Validate(AgentToolJsonSchema.Get(echo)).ShouldBeNull();
    }

    [Fact]
    public void Full_Replace_Put_Input_Warns_And_Patch_Style_Input_Does_Not()
    {
        var logger = new ListLogger<AgentToolCatalog>();
        var catalog = CreateCatalog(o => o.IncludedRemoteServiceNames.Add(DiagnosticsSampleAppService.RemoteServiceName), logger);

        // FullReplaceInput's nullable concurrencyStamp and extraProperties are in the schema, yet do not make it patch-style.
        var input = AgentToolJsonSchema.Get(catalog.Find(DiagnosticsSampleAppService.UpdateFull).ShouldNotBeNull())
            .GetProperty("properties").GetProperty("input").GetProperty("properties");
        input.TryGetProperty("concurrencyStamp", out _).ShouldBeTrue();
        input.TryGetProperty("extraProperties", out _).ShouldBeTrue();

        var warnings = logger.Entries.Where(e => e.Level == LogLevel.Warning).Select(e => e.Message).ToList();
        warnings.ShouldContain(m => m.Contains(DiagnosticsSampleAppService.UpdateFull) && m.Contains("replaces the whole entity")
                                    && m.Contains(typeof(FullReplaceInput).FullName!));
        warnings.ShouldNotContain(m => m.Contains(DiagnosticsSampleAppService.UpdatePatch));
    }

    [Theory]
    [InlineData("""{"type":"object","properties":{"a":{"type":["string","null"]}},"required":["a"]}""", null)]
    [InlineData("""{"type":"object","properties":{"a":{"type":"text"}}}""", "#/properties/a: type")]
    [InlineData("""{"type":"object","properties":{"a":{}},"required":["a","b"]}""", "'b', which is not in properties")]
    [InlineData("""{"type":"object","properties":{"a":{}},"required":["a","a"]}""", "lists 'a' twice")]
    [InlineData("""{"type":"number","minimum":"0"}""", "minimum \"0\" is not a finite JSON number")]
    [InlineData("""{"type":"string","maxLength":-1}""", "maxLength -1 is not a non-negative integer")]
    [InlineData("""{"type":"string","pattern":"(abc"}""", "pattern '(abc' is not a valid regular expression")]
    [InlineData("""{"properties":{"a":{"$ref":"#/properties/b"}}}""", "does not resolve")]
    [InlineData("""{"$defs":{"a":{"$ref":"#/$defs/b"},"b":{"$ref":"#/$defs/a"}},"$ref":"#/$defs/a"}""", "$ref cycle")]
    // Recursion through a property (a tree DTO) and MEAI's repeated-collection shape ($ref with sibling items and maxItems) pass.
    [InlineData("""{"type":"object","properties":{"children":{"type":"array","items":{"$ref":"#"}}}}""", null)]
    [InlineData("""{"type":"object","properties":{"a":{"type":"array","items":{"type":"integer"},"maxItems":3},"b":{"$ref":"#/properties/a","items":{},"maxItems":3}}}""", null)]
    public void Validator_Rules(string schema, string? expectedError)
    {
        var error = AgentToolSchemaValidator.Validate(JsonDocument.Parse(schema).RootElement);

        if (expectedError == null)
        {
            error.ShouldBeNull();
        }
        else
        {
            error.ShouldNotBeNull().ShouldContain(expectedError);
        }
    }
}

// Startup: the module builds the catalogue at application initialization.
public class AgentToolSchemaValidationStartupTests
{
    [Fact]
    public void Host_Starts_Without_The_Invalid_Tool_By_Default()
    {
        using var host = new DiagnosticsHost(failOnInvalidSchema: false);

        host.Logger.Entries.ShouldContain(e => e.Level == LogLevel.Error && e.Message.Contains(DiagnosticsSampleAppService.CreateWithInvalidPattern));
        host.Catalog.Find(DiagnosticsSampleAppService.CreateWithInvalidPattern).ShouldBeNull();
        host.Catalog.Find(DiagnosticsSampleAppService.UpdatePatch).ShouldNotBeNull();
    }

    [Fact]
    public void Host_Start_Fails_With_FailOnInvalidSchema()
    {
        var exception = Should.Throw<Exception>(() => new DiagnosticsHost(failOnInvalidSchema: true));

        var messages = "";
        for (var e = exception; e != null; e = e.InnerException)
        {
            messages += e.Message + "\n";
        }

        messages.ShouldContain(DiagnosticsSampleAppService.CreateWithInvalidPattern);
        messages.ShouldContain("FailOnInvalidSchema");
    }

    private sealed class DiagnosticsHost(bool failOnInvalidSchema) : AgentToolsAspNetCoreTestBase
    {
        // Field initializers and captured primary-constructor parameters are set before the base constructor runs ConfigureServices.
        public ListLogger<AgentToolCatalog> Logger { get; } = new();

        public IAgentToolCatalog Catalog => GetRequiredService<IAgentToolCatalog>();

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger<AgentToolCatalog>>(Logger);
            services.Configure<AbpAgentToolOptions>(o =>
            {
                o.IncludedRemoteServiceNames.Add(DiagnosticsSampleAppService.RemoteServiceName);
                o.FailOnInvalidSchema = failOnInvalidSchema;
            });
        }
    }
}
