using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AgentTools.SchemaSample;

/// <summary>
/// Tools SchemaSample_GetEcho and SchemaSample_GetNextColor (GET: read tools, so MCP lists them too) for the schema and enum
/// tests. A host exposes it as a conventional controller with <see cref="SchemaSampleAppService.RemoteServiceName"/> / <see cref="SchemaSampleAppService.RootPath"/>; tests
/// that want the tools include the remote service themselves, so other tests' tool lists stay as they are.
/// </summary>
public interface ISchemaSampleAppService : IApplicationService
{
    Task<SchemaSampleOutput> GetEchoAsync(SchemaSampleInput input);

    Task<SampleColor> GetNextColorAsync(SampleColor color);

    Task<int> GetPageAsync(SchemaSamplePageInput input);
}

public class SchemaSampleAppService : ISchemaSampleAppService, ITransientDependency
{
    public const string RemoteServiceName = "SchemaSample";
    public const string RootPath = "schema";
    public const string GetEcho = "SchemaSample_GetEcho";
    public const string GetNextColor = "SchemaSample_GetNextColor";
    public const string GetPage = "SchemaSample_GetPage";

    public Task<SchemaSampleOutput> GetEchoAsync(SchemaSampleInput input) => Task.FromResult(new SchemaSampleOutput
    {
        Name = input.Name,
        Color = input.Color,
        LineColors = input.Lines.Select(l => l.Color).ToList()
    });

    public Task<SampleColor> GetNextColorAsync(SampleColor color) => Task.FromResult((SampleColor)(((int)color + 1) % 3));

    public Task<int> GetPageAsync(SchemaSamplePageInput input) => Task.FromResult(input.SkipCount);
}

public enum SampleColor
{
    Red,
    Green,
    Blue
}

public class SchemaSampleInput
{
    [Required]
    [StringLength(20, MinimumLength = 2)]
    public string? Name { get; set; }

    public required string Code { get; set; }

    public int Count { get; set; }

    public int PageSize { get; set; } = 10;

    [DefaultValue(false)]
    public bool Archived { get; set; }

    public int? Limit { get; set; }

    public string? Note { get; set; }

    public int ReadOnlyCount => Count;

    [Range(1, 5)]
    public int? Rating { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal? Price { get; set; }

    [Range(0.5, double.PositiveInfinity)]
    public double? Weight { get; set; }

    [RegularExpression("^[A-Z]{3}$")]
    public string? Currency { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public SampleColor Color { get; set; }

    public SchemaSampleAddress? Address { get; set; }

    public SchemaSamplePoint? Location { get; set; }

    [MinLength(1)]
    [MaxLength(3)]
    public List<SchemaSampleLine> Lines { get; set; } = [];
}

public class SchemaSampleAddress
{
    [Required]
    [MaxLength(50)]
    public string? Street { get; set; }

    public required string City { get; set; }

    public int Number { get; set; }

    [MinLength(4)]
    public string? Zip { get; set; }
}

/// <summary>Bound through its constructor: X has no default (required), Y has one.</summary>
public record SchemaSamplePoint(int X, int Y = 0)
{
    [Required]
    public string? Label { get; init; }
}

public class SchemaSampleLine
{
    [Required]
    public string? Sku { get; set; }

    public required string Unit { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "1000")]
    public decimal? UnitPrice { get; set; }

    public SampleColor Color { get; set; } = SampleColor.Green;
}

/// <summary>ABP's paging and sorting members are optional (SkipCount has no initializer); [Required] still wins.</summary>
public class SchemaSamplePageInput : PagedAndSortedResultRequestDto
{
    [Required]
    public string? Filter { get; set; }

    public SchemaSampleCustomPage? Custom { get; set; }

    public SchemaSampleStrictPage? Strict { get; set; }

    public SchemaSampleNotPaged? NotPaged { get; set; }

    public SchemaSampleExplicitPage? Explicit { get; set; }

    public SchemaSampleDerived? Derived { get; set; }
}

/// <summary>Implements the paging member explicitly: the unrelated public MaxResultCount follows the normal rule.</summary>
public class SchemaSampleExplicitPage : ILimitedResultRequest
{
    int ILimitedResultRequest.MaxResultCount { get; set; } = 10;

    public int MaxResultCount { get; set; }
}

public class SchemaSampleBase
{
    [Required]
    public virtual string? Title { get; set; }

    [DefaultValue(1)]
    public virtual int Level { get; set; }
}

/// <summary>Overrides without repeating the attributes: Title stays required, Level keeps its [DefaultValue].</summary>
public class SchemaSampleDerived : SchemaSampleBase
{
    public override string? Title { get; set; }

    public override int Level { get; set; }
}

/// <summary>A paged input of its own: its paging members are optional, other value types are not.</summary>
public class SchemaSampleCustomPage : IPagedResultRequest
{
    public int SkipCount { get; set; }

    public int MaxResultCount { get; set; } = 20;

    public int Level { get; set; }
}

/// <summary>Paging members redeclared as explicitly required stay required.</summary>
public class SchemaSampleStrictPage : PagedAndSortedResultRequestDto
{
    [Required]
    public override int SkipCount { get; set; }

    public override required int MaxResultCount { get; set; }

    [Required]
    public override string? Sorting { get; set; }
}

/// <summary>Not an ABP paged input: a property named SkipCount follows the normal rule.</summary>
public class SchemaSampleNotPaged
{
    public int SkipCount { get; set; }
}

public class SchemaSampleOutput
{
    public string? Name { get; set; }

    public SampleColor Color { get; set; }

    public List<SampleColor> LineColors { get; set; } = [];
}

/// <summary>What the schema of <see cref="SchemaSampleAppService.GetEcho"/> must say, wherever it is read (chat function, MCP).</summary>
public static class SchemaSampleAssertions
{
    public static void ShouldDescribeTheEchoInput(JsonElement schema, bool requireNonNullableValueTypes = true)
    {
        ShouldHaveOnlyFiniteNumericBounds(schema);
        schema.GetProperty("required").EnumerateArray().Select(e => e.GetString()).ShouldBe(["input"]);

        var input = schema.GetProperty("properties").GetProperty("input");
        Required(input).ShouldBe(requireNonNullableValueTypes ? ["name", "code", "count", "color"] : ["name", "code"]);
        var properties = input.GetProperty("properties");
        properties.GetProperty("name").GetProperty("minLength").GetInt32().ShouldBe(2);
        properties.GetProperty("name").GetProperty("maxLength").GetInt32().ShouldBe(20);
        properties.GetProperty("rating").GetProperty("minimum").GetInt32().ShouldBe(1);
        properties.GetProperty("rating").GetProperty("maximum").GetInt32().ShouldBe(5);
        // Decimal [Range]: JSON numbers, not the attribute's strings.
        properties.GetProperty("price").GetProperty("minimum").GetDouble().ShouldBe(0);
        properties.GetProperty("price").GetProperty("maximum").GetDouble().ShouldBe((double)decimal.MaxValue);
        // Infinite bound left out, the finite one kept.
        properties.GetProperty("weight").GetProperty("minimum").GetDouble().ShouldBe(0.5);
        properties.GetProperty("weight").TryGetProperty("maximum", out _).ShouldBeFalse();
        properties.GetProperty("currency").GetProperty("pattern").GetString().ShouldBe("^[A-Z]{3}$");
        properties.GetProperty("email").GetProperty("format").GetString().ShouldBe("email");
        Strings(properties.GetProperty("color"), "enum").ShouldBe(["Red", "Green", "Blue"]);
        properties.GetProperty("color").GetProperty("type").GetString().ShouldBe("string");

        var address = properties.GetProperty("address");
        Required(address).ShouldBe(requireNonNullableValueTypes ? ["street", "city", "number"] : ["street", "city"]);
        address.GetProperty("properties").GetProperty("street").GetProperty("maxLength").GetInt32().ShouldBe(50);
        address.GetProperty("properties").GetProperty("zip").GetProperty("minLength").GetInt32().ShouldBe(4);

        // Constructor parameters: required without a default (System.Text.Json's own rule, whatever the option).
        Required(properties.GetProperty("location")).ShouldBe(["x", "label"]);

        var lines = properties.GetProperty("lines");
        lines.GetProperty("minItems").GetInt32().ShouldBe(1);
        lines.GetProperty("maxItems").GetInt32().ShouldBe(3);
        var line = lines.GetProperty("items");
        Required(line).ShouldBe(requireNonNullableValueTypes ? ["sku", "unit", "quantity"] : ["sku", "unit"]);
        line.GetProperty("properties").GetProperty("quantity").GetProperty("minimum").GetInt32().ShouldBe(1);
        line.GetProperty("properties").GetProperty("quantity").GetProperty("maximum").GetInt32().ShouldBe(100);
        line.GetProperty("properties").GetProperty("unitPrice").GetProperty("minimum").GetDouble().ShouldBe(0.01);
        line.GetProperty("properties").GetProperty("unitPrice").GetProperty("maximum").GetDouble().ShouldBe(1000);
        Strings(line.GetProperty("properties").GetProperty("color"), "enum").ShouldBe(["Red", "Green", "Blue"]);
    }

    /// <summary>What the schema of <see cref="SchemaSampleAppService.GetPage"/> must say, wherever it is read (chat function, MCP).</summary>
    public static void ShouldHaveOptionalPagingMembers(JsonElement schema)
    {
        var input = schema.GetProperty("properties").GetProperty("input");
        Required(input).ShouldBe(["filter"]);
        var properties = input.GetProperty("properties");
        properties.TryGetProperty("skipCount", out _).ShouldBeTrue();
        properties.TryGetProperty("sorting", out _).ShouldBeTrue();
        Required(properties.GetProperty("custom")).ShouldBe(["level"]);
        Required(properties.GetProperty("strict")).ShouldBe(["skipCount", "maxResultCount", "sorting"], ignoreOrder: true);
        Required(properties.GetProperty("notPaged")).ShouldBe(["skipCount"]);
        Required(properties.GetProperty("explicit")).ShouldBe(["maxResultCount"]);
        Required(properties.GetProperty("derived")).ShouldBe(["title"]);
    }

    /// <summary>Every numeric keyword anywhere in the schema is a JSON number.</summary>
    public static void ShouldHaveOnlyFiniteNumericBounds(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                ShouldHaveOnlyFiniteNumericBounds(item);
            }
        }
        else if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (property.Name is "minimum" or "maximum" or "exclusiveMinimum" or "exclusiveMaximum"
                    or "minLength" or "maxLength" or "minItems" or "maxItems")
                {
                    property.Value.ValueKind.ShouldBe(JsonValueKind.Number, property.Name);
                    double.IsFinite(property.Value.GetDouble()).ShouldBeTrue(property.Name);
                }

                ShouldHaveOnlyFiniteNumericBounds(property.Value);
            }
        }
    }

    private static string?[] Required(JsonElement schema) =>
        schema.TryGetProperty("required", out var required) ? required.EnumerateArray().Select(e => e.GetString()).ToArray() : [];

    private static string?[] Strings(JsonElement schema, string name) => schema.GetProperty(name).EnumerateArray().Select(e => e.GetString()).ToArray();
}
