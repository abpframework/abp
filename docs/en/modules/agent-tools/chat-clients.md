```json
//[doc-seo]
{
    "Description": "Give an IChatClient the current user's governed agent tools with IAgentToolProvider, handle approvals, call tools by name with IAgentToolInvoker and observe every call."
}
```

# Agent Tools: Using Tools from Your Own Chat Client

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use this module.

The tools are [Microsoft.Extensions.AI](../../framework/infrastructure/artificial-intelligence/microsoft-extensions-ai.md) `AIFunction`s, so any `IChatClient` can use them. Your code runs in the host that runs the application services (it needs `AgentToolsAspNetCoreModule`, see [Installation](./installation.md)).

## Getting the Current User's Tools

`IAgentToolProvider.GetToolsAsync(context)` returns the tools the current user may use, in the current tenant. A tool is left out when:

- the [policy](./policy.md) disables it;
- it requires approval and `context.SupportsApproval` is `false`;
- the user lacks its permissions or its required features, as far as ABP's attributes (`[Authorize]`, `[RequiresFeature]`) and the policy names of ABP's CRUD application services (`GetPolicyName`, `GetListPolicyName`, `CreatePolicyName`, `UpdatePolicyName`, `DeletePolicyName`) tell;
- the `AgentTools.Enable` feature is off.

The list keeps the model from trying tools it cannot use. It is not the security boundary: every call is checked again by ABP when it runs.

`AgentToolContext`:

| Property | Meaning |
| --- | --- |
| `SupportsApproval` | `true` when your client asks a human before running a tool that requires approval. `false` hides and refuses those tools. |
| `ClientId`, `ClientName` | Written to the audit log comment and the call record. |

Each `AgentTool` has:

| Property | Meaning |
| --- | --- |
| `Definition` | The `AgentToolDefinition`: name, description, kind, annotations, HTTP method and URL, remote service, service interface and parameters. |
| `RequiresApproval` | The effective policy value. |
| `Function` | The `AIFunction` to give to an `IChatClient`. When the tool requires approval, it is wrapped in Microsoft.Extensions.AI's `ApprovalRequiredAIFunction`. |
| `UnwrappedFunction` | The plain function, never wrapped, for hosts with their own approval flow. |

## A Minimal Chat

This service answers a question with the current user's read tools. The `IChatClient` must invoke functions, so the sample adds `UseFunctionInvocation()` (package `Microsoft.Extensions.AI`); skip it if your configured chat client already has it.

```csharp
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Volo.Abp.DependencyInjection;
using Volo.AgentTools;

namespace MyProjectName.Chat;

public class BookAssistant : ITransientDependency
{
    private readonly IAgentToolProvider _toolProvider;
    private readonly IChatClient _chatClient;

    public BookAssistant(IAgentToolProvider toolProvider, IChatClient chatClient)
    {
        _toolProvider = toolProvider;
        _chatClient = chatClient.AsBuilder().UseFunctionInvocation().Build();
    }

    public async Task<string> AskAsync(string question)
    {
        var tools = await _toolProvider.GetToolsAsync(new AgentToolContext
        {
            SupportsApproval = false, // no approval UI: tools that require approval are left out
            ClientId = "book-assistant",
            ClientName = "Book assistant"
        });

        var options = new ChatOptions
        {
            Tools = tools.Select(tool => (AITool)tool.Function).ToList()
        };

        var response = await _chatClient.GetResponseAsync(question, options);
        return response.Text;
    }
}
```

The tools run as the user and tenant that are current when the model calls them, so call `AskAsync` in the request of the signed-in user. See [Microsoft.Extensions.AI](../../framework/infrastructure/artificial-intelligence/microsoft-extensions-ai.md) for configuring the `IChatClient`.

> Do not expose a class like this as an application service that the catalogue includes: it would become a tool itself. Keep it out of the included remote services, or mark it with `[DisableAgentTool]`.

## Approvals

With `SupportsApproval = true` the provider also returns the tools that require approval, wrapped in `ApprovalRequiredAIFunction`. Microsoft.Extensions.AI's `FunctionInvokingChatClient` then does not call such a tool; it ends the turn with a `FunctionApprovalRequestContent`. Show the request to the user, and send the decision back with the conversation:

```csharp
#pragma warning disable MEAI001 // The approval types of Microsoft.Extensions.AI are experimental
var messages = new List<ChatMessage> { new(ChatRole.User, question) };
var response = await _chatClient.GetResponseAsync(messages, options);

var requests = response.Messages
    .SelectMany(message => message.Contents)
    .OfType<FunctionApprovalRequestContent>()
    .ToList();

if (requests.Count > 0)
{
    messages.AddRange(response.Messages);

    var decisions = new List<AIContent>();
    foreach (var request in requests)
    {
        // Show request.FunctionCall.Name and request.FunctionCall.Arguments to the user.
        var approved = await AskTheUserAsync(request.FunctionCall);
        decisions.Add(request.CreateResponse(approved));
    }

    messages.Add(new ChatMessage(ChatRole.User, decisions));
    response = await _chatClient.GetResponseAsync(messages, options);
}
#pragma warning restore MEAI001
```

`AskTheUserAsync` stands for your own UI. Setting `SupportsApproval = true` is a promise that your client asks a human for these tools; the module cannot verify it.

A tool's function remembers whether it was listed as requiring approval. When the policy starts requiring approval after the list was built, a call through that function is refused (`AgentTools:ApprovalPolicyChanged`), even with `SupportsApproval = true`: get the tool list again.

## Calling a Tool by Name

`IAgentToolInvoker.InvokeAsync(toolName, arguments, context)` calls a tool directly. It is what the tool functions call.

```csharp
var result = await _toolInvoker.InvokeAsync(
    "Book_Get",
    new Dictionary<string, object?> { ["id"] = bookId },
    new AgentToolContext { ClientId = "book-assistant" });
```

The invoker:

- resolves the application service interface in a new scope and calls the interface method, never the implementation or the controller directly;
- runs the call in its own transactional unit of work, so a failure rolls it back;
- binds arguments by parameter name, and fails with one validation error that names every missing or invalid argument;
- does not throw for tool failures: the result is the JSON of the return value (`{"success":true}` for a method without one), or an error line.

Error lines have the form `ERROR <ExceptionType>: <message>`. The message is what ABP would send an HTTP client (localized, without internal details or stack traces), with validation messages and the error code when there is one, for example:

```text
ERROR AbpAuthorizationException: Authorization failed! Given policy has not granted. (code: Volo.Authorization:010001)
ERROR AbpValidationException: Your request is not valid! Validation errors: [Name] The field Name must be a string with a maximum length of 128.
```

The invoker refuses before it calls the service with these error codes:

| Code | Reason |
| --- | --- |
| `AgentTools:ToolNotFound` | There is no tool with this name. |
| `AgentTools:ToolDisabled` | The policy disables the tool. |
| `AgentTools:ApprovalNotSupported` | The tool requires approval and `SupportsApproval` is `false`. |
| `AgentTools:ApprovalPolicyChanged` | The tool was listed without approval, and the policy requires it now. |
| `AgentTools:FeatureDisabled` | The `AgentTools.Enable` feature is off. |

A missing permission is not refused by the invoker itself: the call goes on, and ABP's authorization refuses and audits it. Cancelling the caller's `CancellationToken` throws `OperationCanceledException`.

## Observing Calls

Every registered `IAgentToolCallObserver` receives one `AgentToolCallRecord` per call, from chat clients and from the MCP endpoint:

```csharp
public class LoggingAgentToolCallObserver : IAgentToolCallObserver
{
    private readonly ILogger<LoggingAgentToolCallObserver> _logger;

    public LoggingAgentToolCallObserver(ILogger<LoggingAgentToolCallObserver> logger)
    {
        _logger = logger;
    }

    public Task OnCallAsync(AgentToolCallRecord record)
    {
        _logger.LogInformation(
            "Agent tool {Tool} called by {User} through {Client}: {Outcome} in {Duration} ms",
            record.ToolName,
            record.UserName,
            record.ClientId,
            record.Succeeded ? "succeeded" : record.ErrorCode ?? record.ErrorType,
            record.Duration.TotalMilliseconds);

        return Task.CompletedTask;
    }
}
```

Register it in `ConfigureServices`:

```csharp
context.Services.AddTransient<IAgentToolCallObserver, LoggingAgentToolCallObserver>();
```

The record has `ToolName`, `Arguments` (JSON, with sensitive values redacted), `Succeeded`, `ErrorType`, `ErrorCode`, `ErrorMessage`, `StartTime`, `Duration`, `UserId`, `UserName`, `TenantId`, `ClientId` and `ClientName`. An observer that throws is logged and never breaks the call.

When an audit log scope is active (it is in HTTP requests), the invoker also adds the comment `Agent tool <name> (client: <client id>; <client name>)` to the [audit log](../../framework/infrastructure/audit-logging.md).
