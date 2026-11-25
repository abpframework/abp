```json
//[doc-seo]
{
    "Description": "Choose which ABP application services become agent tools, and control tool names, descriptions, annotations and input schemas with AbpAgentToolOptions and the AgentTool attributes."
}
```

# Agent Tools: Exposing Tools

The `Volo.Abp.AgentTools.AspNetCore` package (module class `AbpAgentToolsAspNetCoreModule`) turns HTTP API actions of your application into tools; see [First Steps](./index.md#first-steps) to install it. A tool is one action. The module reads the actions from ABP's API description model (the model that conventional controllers and [client proxies](../../../api-development/dynamic-csharp-clients.md) use), and calls each one in-process through its application service interface. Exposure is opt-in: with nothing included the catalog is empty.

## Choosing What to Expose

Configure `AbpAgentToolOptions` in the `ConfigureServices` method of the module that hosts the application services:

```csharp
Configure<AbpAgentToolOptions>(options =>
{
    options.IncludedRemoteServiceNames.Add("Default");       // by remote service name
    options.IncludedRootPaths.Add("identity");               // by module root path
    options.IncludedServiceTypes.Add(type => type.Namespace == "MyProjectName.Books");
    options.ExcludedServiceTypes.Add(type => type == typeof(IReportAppService));
});
```

An action is a candidate when any include matches. A match in `ExcludedServiceTypes` removes the action whatever included it.

| Property | Default | Meaning |
| --- | --- | --- |
| `IncludedRemoteServiceNames` | empty | Remote service names (`[RemoteService(Name = ...)]`, or the conventional controller setting; `"Default"` for most application services). Case-insensitive. |
| `IncludedRootPaths` | empty | Module root paths of the API description model, such as `"app"` or `"identity"`. Case-insensitive. |
| `IncludedServiceTypes` | empty | Predicates over the service interface (over the controller type when the action has no interface). |
| `ExcludedServiceTypes` | empty | The same predicates, for exclusion. |
| `MaxTools` | `150` | Upper bound for the number of tools. More included actions make the application fail at startup. |
| `RequireNonNullableValueTypes` | `true` | Input schemas list a non-nullable value-type DTO property without a default as required (see [Input Schemas](#input-schemas)). |
| `FailOnInvalidSchema` | `false` | A tool whose input schema fails validation makes startup fail instead of being left out with an error log. |
| `AdditionalSensitiveArgumentNames` | empty | Extra name parts whose argument values are redacted in logs and call records (see [Security Notes](./security.md#redaction)). |

The catalog is built once, at application startup. `MaxTools` fails fast with an `AbpException` that gives the number of included actions. Narrow the includes, or exclude services, rather than raising the limit: models choose tools less reliably from very large lists.

### What Is Never Exposed

- Anonymous actions: `[AllowAnonymous]` on the interface method, the controller action, the implementation method or the implementation class.
- Integration services (`[IntegrationService]`).
- Actions with non-JSON input: form binding, file uploads, `Stream`, `IRemoteStreamContent` and `IFormFile`, also as a property of the input class.
- Actions with a parameter from services or from a header (`[FromServices]`, `[FromKeyedServices]` or `[FromHeader]`), or with a `[FromHeader]` property in a query DTO. Model binders and custom binding sources are no reason to skip an action (see [Security Notes](./security.md#discovery-and-schemas)).
- Actions that return a stream, a file or an `IActionResult`.
- Methods, classes and interfaces marked `[DisableAgentTool]`.
- Controller actions without an application service interface. A hand-written controller is exposed through the application service interface it implements.
- Service interfaces that are not registered in dependency injection or resolve to a controller.
- Services with `[RemoteService(false)]`: they are not in the API description model.

Included actions that are skipped are listed in `IAgentToolCatalog.Skipped` with the reason, and logged when the catalog is built.

## Names

- A tool's name is `Controller_Action`: the controller name and the action name of the API description model, with the `Async` suffix removed. For example `Book_GetList`. Characters other than `A-Z`, `a-z`, `0-9`, `_` and `-` become `_`.
- Overloads in one controller get the suffixes `_2`, `_3`, ... in interface declaration order.
- Names are at most 64 characters.
- `[AgentTool(Name = "...")]` sets the name. It must match `^[A-Za-z0-9_-]{1,64}$`.
- Names are unique without case. Two tools with the same name, or with names that differ only by case, make the application fail at startup, with a message that names both. Rename one with `[AgentTool(Name = ...)]` or exclude it.

## Descriptions

The model chooses tools by their descriptions, so write them for the model. The first match wins:

1. `[AgentTool(Description = "...")]`.
2. `[Description]` (`System.ComponentModel`) on the interface method, then on the controller method, then on the implementation method.
3. The XML documentation summary of the action, when the API description model has it.
4. A generated sentence, for example `Calls BookAppService.Get (GET api/app/book/{id}).`

Parameter descriptions come from `[Description]` on the parameter, then from the API description model.

## Kind, Annotations and Approval

The HTTP verb decides a tool's kind, its annotations and whether it requires approval by default. The annotations are exposed on the `AIFunction` and in the MCP tool list as `readOnlyHint`, `destructiveHint` and `idempotentHint`.

| Verb | Kind | Read-only | Destructive | Idempotent | Requires approval by default |
| --- | --- | --- | --- | --- | --- |
| GET | `Read` | yes | no | yes | no |
| POST | `Write` | no | no | no | yes |
| PUT | `Write` | no | no | yes | yes |
| PATCH | `Write` | no | no | no | yes |
| DELETE | `Destructive` | no | yes | yes | yes |

The [tool policy](./policy.md) can change whether a tool is enabled and whether it requires approval.

## Attributes

The attributes are in the `Volo.Abp.AgentTools.Abstractions` package (module class `AbpAgentToolsAbstractionsModule`, namespace `Volo.Abp.AgentTools`), which has no ASP.NET Core dependency. Add it to the project that defines the application service interfaces (`{ProjectName}.Application.Contracts`).

```csharp
public interface IBookAppService : IApplicationService
{
    [Description("Lists books whose name contains the filter text.")]
    Task<PagedResultDto<BookDto>> GetListAsync(GetBooksInput input);

    [AgentTool(Name = "Book_SetPrice", Description = "Changes the price of one book.")]
    Task<BookDto> SetPriceAsync(Guid id, [Description("The new price.")] decimal price);

    [DisableAgentTool]
    Task DeleteAllAsync();
}
```

### AgentTool

`[AgentTool]` on a method overrides what the catalog derives. Its properties are `Name`, `Description`, `RequiresApproval`, `ReadOnly`, `Destructive` and `Idempotent`. A property that is not set keeps the derived value.

The attribute can be on the interface method, the controller method or the implementation method. The values are merged per property: the implementation method wins over the controller method, which wins over the interface method.

`ReadOnly = true` implies not destructive and idempotent; `Destructive = true` implies not read-only, unless the other is set explicitly. The kind follows: destructive, else read-only, else write.

`RequiresApproval` sets the code default of the policy. For example, `[AgentTool(RequiresApproval = false)]` on a harmless write makes it available over MCP without a policy change.

### DisableAgentTool

`[DisableAgentTool]` on a method, class or interface removes it from the catalog, whatever the options include. It is checked on the controller, the service interface, the implementation class and their methods.

## Input Schemas

Each tool has one JSON schema for its input, built from the interface method's parameters with camelCase names. Chat clients and the MCP tool list get the same schema.

- **Method parameters** are required when they have no default value.
- **DTO properties** (also in nested objects and collection items) are required when they have `[Required]`, the C# `required` modifier or `[JsonRequired]`, or are bound to a constructor parameter without a default (records).
- With `RequireNonNullableValueTypes` (the default), a settable non-nullable value type (`int`, `bool`, `decimal`, `Guid`, `DateTime`, an enum, a struct) is required too, unless it has a default: a property initializer (`public int PageSize { get; set; } = 10;`) or `[DefaultValue]`. An initializer equal to the type's default (`= 0`) cannot be detected; use a nullable type or `[DefaultValue]` for such an optional value.
- **Paging and sorting members** of ABP's request DTOs (`MaxResultCount`, `SkipCount` and `Sorting` of a type that implements `ILimitedResultRequest`, `IPagedResultRequest` or `ISortedResultRequest`, such as `PagedAndSortedResultRequestDto`) are always optional, unless the member itself has `[Required]`, `[JsonRequired]` or `required`.
- **Data annotations**: `[StringLength]`, `[MinLength]` and `[MaxLength]` give `minLength` / `maxLength` on strings and `minItems` / `maxItems` on collections; `[Range]` gives `minimum` / `maximum`; `[RegularExpression]` gives `pattern`; `[EmailAddress]` gives `format: email`.
- **Enums** are strings that list the member names (`"enum": ["Red", "Green"]`). Enum arguments are accepted as names (ignoring case) or numbers, and results carry the names.

> [!NOTE]
> With `RequireNonNullableValueTypes` enabled, a plain `bool` or enum property without an initializer in a create DTO is required in the schema. Give it an initializer or `[DefaultValue]` if the model may leave it out.

### Schema Validation

One invalid schema makes an AI provider reject every request that carries it. So the catalog checks each schema when it is built, against the subset that providers accept: valid `type` values, `required` names that exist, numeric bounds that are finite numbers, patterns that parse as .NET regular expressions, and local `$ref`s that resolve.

A tool that fails is left out of the catalog, so neither chat clients nor MCP offer it. An error log names the tool and the reason, and the tool is listed in `IAgentToolCatalog.Skipped`. Set `FailOnInvalidSchema` to `true` to make startup fail instead:

```csharp
Configure<AbpAgentToolOptions>(options =>
{
    options.FailOnInvalidSchema = true;
});
```

Providers use ECMAScript-like regular expressions. Keep agent-facing patterns to the common subset: .NET-only syntax such as inline options (`(?i)`) passes the check but may be rejected by a provider.

## Update DTOs: Prefer Patch-Style

An agent that changes one field of an entity sends only that field. With a full-replace input DTO (every property required, as a `CreateUpdate...Dto` usually is), the other fields arrive empty and the update clears them.

When the catalog is built, a `PUT` or `PATCH` tool whose input object has no optional property besides `id` and `concurrencyStamp` logs a warning that names the tool.

Use a patch-style DTO for agent tools instead: nullable properties, where `null` means "leave unchanged".

```csharp
public class UpdateBookPatchDto
{
    [StringLength(128)]
    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public BookType? Type { get; set; }
}
```

```csharp
public async Task<BookDto> UpdateAsync(Guid id, UpdateBookPatchDto input)
{
    var book = await _bookRepository.GetAsync(id);

    if (input.Name != null)
    {
        book.Name = input.Name;
    }

    if (input.Price != null)
    {
        book.Price = input.Price.Value;
    }

    if (input.Type != null)
    {
        book.Type = input.Type.Value;
    }

    await _bookRepository.UpdateAsync(book);
    return ObjectMapper.Map<Book, BookDto>(book);
}
```

A field that may be set to `null` needs a separate signal (a `Clear...` flag or a tool of its own), because `null` already means "leave unchanged". If the UI needs the full-replace endpoint, keep it and mark it with `[DisableAgentTool]`.

> [!WARNING]
> Do not expose a full-replace update to agents. A model that sends a partial object through it erases the fields it left out.
