using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.AgentTools.Sample;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Host with "AgentToolsSample" and "OtherSample" included, an <see cref="InMemoryAgentToolPolicyStore"/> as the policy
/// store, a recording observer behind a throwing one, and a list logger for the invoker.
/// Tool calls run in the invoker's own unit of work; never wrap them in an outer transactional one (single Sqlite writer).
/// </summary>
public abstract class AgentToolsInvocationTestBase : AgentToolsAspNetCoreTestBase
{
    protected static readonly AgentToolContext WithApproval = new() { SupportsApproval = true, ClientId = "test-client", ClientName = "Test Client" };

    protected static readonly AgentToolContext WithoutApproval = new() { SupportsApproval = false, ClientId = "test-client" };

    // Field initializers run before the base constructor, which builds the host (ConfigureServices).
    protected ListLogger<AgentToolInvoker> InvokerLogger { get; } = new();

    protected RecordingObserver Observer { get; } = new();

    protected InMemoryAgentToolPolicyStore PolicyStore => GetRequiredService<InMemoryAgentToolPolicyStore>();

    protected IAgentToolInvoker Invoker => GetRequiredService<IAgentToolInvoker>();

    protected IAgentToolProvider Provider => GetRequiredService<IAgentToolProvider>();

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpAgentToolOptions>(o =>
        {
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.OtherSample);
        });

        services.AddSingleton<InMemoryAgentToolPolicyStore>();
        services.Replace(ServiceDescriptor.Singleton<IAgentToolPolicyStore>(sp => sp.GetRequiredService<InMemoryAgentToolPolicyStore>()));

        services.AddSingleton<IAgentToolCallObserver, ThrowingObserver>();
        services.AddSingleton<IAgentToolCallObserver>(Observer);
        services.AddSingleton<ILogger<AgentToolInvoker>>(InvokerLogger);
    }

    protected Task<string> InvokeAsync(string toolName, object? arguments = null, AgentToolContext? context = null)
    {
        return Invoker.InvokeAsync(toolName, Args(arguments), context ?? WithApproval);
    }

    /// <summary>Arguments as a model sends them: JsonElement values per top-level property.</summary>
    protected static AIFunctionArguments Args(object? values = null)
    {
        var arguments = new AIFunctionArguments();
        if (values != null)
        {
            foreach (var property in JsonSerializer.SerializeToElement(values).EnumerateObject())
            {
                arguments[property.Name] = property.Value.Clone();
            }
        }

        return arguments;
    }

    protected static JsonElement Json(string result)
    {
        result.ShouldNotStartWith("ERROR", customMessage: result);
        return JsonSerializer.Deserialize<JsonElement>(result);
    }

    public class RecordingObserver : IAgentToolCallObserver
    {
        public ConcurrentQueue<AgentToolCallRecord> Records { get; } = new();

        public Task OnCallAsync(AgentToolCallRecord record)
        {
            Records.Enqueue(record);
            return Task.CompletedTask;
        }
    }

    private class ThrowingObserver : IAgentToolCallObserver
    {
        public Task OnCallAsync(AgentToolCallRecord record) => throw new InvalidOperationException("observer failure");
    }
}
