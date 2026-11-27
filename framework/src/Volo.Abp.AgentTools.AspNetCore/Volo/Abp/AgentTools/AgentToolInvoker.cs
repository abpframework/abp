using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Aspects;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.DynamicProxy;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Logging;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.Abp.Validation;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Calls a tool through its service interface resolved from DI, in its own transactional unit of work, and turns every
/// outcome into a string for the model: the result as JSON, or "ERROR &lt;ExceptionType&gt;: &lt;message&gt;".
/// Reports each call to all <see cref="IAgentToolCallObserver"/>s.
/// </summary>
public class AgentToolInvoker : IAgentToolInvoker
{
    /// <summary>Result of a successful call to a method without a return value.</summary>
    public const string VoidResultJson = "{\"success\":true}";

    public const string RedactedValue = AgentToolArgumentRedactor.RedactedValue;

    /// <summary>Longer names are cut in logs, audit comments, call records and messages.</summary>
    public const int MaxShownNameLength = 128;

    private static readonly ConditionalWeakTable<JsonSerializerOptions, JsonSerializerOptions> ToolJsonOptions = new();

    protected IAgentToolCatalog Catalog { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected ILogger<AgentToolInvoker> Logger { get; }

    /// <summary>Options of the module; null (not injected) means the built-in redaction list only.</summary>
    protected IOptions<AbpAgentToolOptions>? Options { get; }

    public AgentToolInvoker(
        IAgentToolCatalog catalog,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AgentToolInvoker> logger,
        IOptions<AbpAgentToolOptions>? options = null)
    {
        Catalog = catalog;
        ServiceScopeFactory = serviceScopeFactory;
        Logger = logger;
        Options = options;
    }

    public virtual async Task<string> InvokeAsync(
        string toolName,
        IDictionary<string, object?> arguments,
        AgentToolContext context,
        CancellationToken cancellationToken = default)
    {
        // An unknown name is whatever the caller sent: as it is for the lookup only.
        var requestedName = toolName;
        toolName = GetShownName(toolName);

        // A scope per call: the service, its dependencies and our helpers are disposed with the call, whoever holds
        // the invoker. User, tenant, unit of work and audit scope are ambient, so they flow into it.
        using var scope = ServiceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var jsonOptions = GetToolJsonOptions(services.GetRequiredService<IOptions<AbpSystemTextJsonSerializerOptions>>().Value.JsonSerializerOptions);
        var startTime = services.GetRequiredService<IClock>().Now;
        var stopwatch = Stopwatch.StartNew();
        var redactedArguments = RedactArguments(arguments, jsonOptions);

        AddAuditComment(services, toolName, context);

        string result;
        Exception? error = null;
        string? errorMessage = null;
        ExceptionDispatchInfo? cancellation = null;
        try
        {
            result = await InvokeCoreAsync(services, requestedName, toolName, arguments, context, jsonOptions, cancellationToken);
        }
        catch (Exception ex) when (ex is OperationCanceledException && cancellationToken.IsCancellationRequested)
        {
            // The caller's own cancellation: recorded once like any call, then rethrown below.
            cancellation = ExceptionDispatchInfo.Capture(ex);
            error = ex;
            errorMessage = "The call was cancelled.";
            result = "";
        }
        catch (Exception ex)
        {
            error = ex;
            errorMessage = FormatErrorMessage(services, ex);
            result = $"ERROR {ex.GetType().Name}: {errorMessage}";
        }

        stopwatch.Stop();

        if (cancellation != null)
        {
            Logger.LogInformation("Agent tool {ToolName} was cancelled after {Duration} ms (client: {ClientId}). Arguments: {Arguments}",
                toolName, stopwatch.Elapsed.TotalMilliseconds, context.ClientId, redactedArguments);
        }
        else if (error == null)
        {
            Logger.LogInformation("Agent tool {ToolName} succeeded in {Duration} ms (client: {ClientId}). Arguments: {Arguments}",
                toolName, stopwatch.Elapsed.TotalMilliseconds, context.ClientId, redactedArguments);
        }
        else
        {
            Logger.Log((error as IHasLogLevel)?.LogLevel ?? LogLevel.Error, error,
                "Agent tool {ToolName} failed in {Duration} ms (client: {ClientId}): {Result}. Arguments: {Arguments}",
                toolName, stopwatch.Elapsed.TotalMilliseconds, context.ClientId, result, redactedArguments);
        }

        var currentUser = services.GetRequiredService<ICurrentUser>();
        await NotifyObserversAsync(services, new AgentToolCallRecord
        {
            ToolName = toolName,
            Arguments = redactedArguments,
            Succeeded = error == null,
            ErrorType = error?.GetType().FullName,
            ErrorCode = (error as IHasErrorCode)?.Code,
            ErrorMessage = errorMessage,
            StartTime = startTime,
            Duration = stopwatch.Elapsed,
            UserId = currentUser.Id,
            UserName = currentUser.UserName,
            TenantId = services.GetRequiredService<ICurrentTenant>().Id,
            ClientId = context.ClientId,
            ClientName = context.ClientName
        });

        cancellation?.Throw();
        return result;
    }

    protected virtual async Task<string> InvokeCoreAsync(
        IServiceProvider services,
        string requestedName,
        string toolName,
        IDictionary<string, object?> arguments,
        AgentToolContext context,
        JsonSerializerOptions jsonOptions,
        CancellationToken cancellationToken)
    {
        if (!await services.GetRequiredService<IFeatureChecker>().IsEnabledAsync(AgentToolsFeatures.Enable))
        {
            throw Refusal(AgentToolsErrorCodes.FeatureDisabled, "Agent tools are not enabled for the current tenant.", toolName);
        }

        var definition = Catalog.Find(requestedName)
                         ?? throw Refusal(AgentToolsErrorCodes.ToolNotFound, $"There is no agent tool named '{toolName}'.", toolName);

        var policy = await services.GetRequiredService<IAgentToolPolicyStore>().GetAsync(definition, cancellationToken);
        if (!policy.Enabled)
        {
            throw Refusal(AgentToolsErrorCodes.ToolDisabled, $"The agent tool '{toolName}' is disabled.", toolName);
        }

        // The invoker cannot see whether a human approved; a context with SupportsApproval promises it asked.
        if (policy.RequiresApproval && !context.SupportsApproval)
        {
            throw Refusal(AgentToolsErrorCodes.ApprovalNotSupported,
                $"The agent tool '{toolName}' requires human approval, which this client does not support.", toolName);
        }

        // Stale list: the function was handed out approval-free, so no human was asked; the policy changed since.
        if (policy.RequiresApproval && context.ListedRequiresApproval == false)
        {
            throw Refusal(AgentToolsErrorCodes.ApprovalPolicyChanged,
                $"The agent tool '{toolName}' now requires human approval, but it was listed without it. Get the tool list again.", toolName);
        }

        // Not permitted is NOT refused here: the call goes on so ABP's own authorization refuses it (and audits it).

        // Own transactional unit of work per call; disposed without CompleteAsync on an exception = rollback.
        using var uow = services.GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true, isTransactional: true);

        cancellationToken.ThrowIfCancellationRequested();

        var service = services.GetRequiredService(definition.ServiceInterface);
        var parameterValues = BindArguments(definition, arguments, jsonOptions, cancellationToken);

        await AgentToolAuthorizationChecks.CheckControllerAsync(services, definition);

        // The real cause decides, not only the instance: an ignored type has no ABP interceptors even if something else
        // proxies it. When both hold the checks may run twice, which is harmless.
        var returnValue = ProxyHelper.IsProxy(service) && !DynamicProxyIgnoreTypes.Contains(definition.ImplementationType)
            ? await InvokeMethodAsync(definition, service, parameterValues)
            : await InvokeUnproxiedAsync(services, definition, service, parameterValues);

        // Serialized inside the unit of work: lazy members and serialization errors still roll the call back.
        var returnType = AsyncHelper.UnwrapTask(definition.InterfaceMethod.ReturnType);
        var result = returnType == typeof(void) ? VoidResultJson : JsonSerializer.Serialize(returnValue, returnType, jsonOptions);

        await uow.CompleteAsync(cancellationToken);
        return result;
    }

    /// <summary>
    /// ABP framework behaviour: <c>AbpAspNetCoreMvcModule.PostConfigureServices</c> adds every
    /// conventional controller type registered through <c>PreConfigure&lt;AbpAspNetCoreMvcOptions&gt;</c> (the documented way) to
    /// <c>DynamicProxyIgnoreTypes</c>, because MVC filters do the cross-cutting work for HTTP calls. Such application services
    /// are not proxied. Services exposed by <c>ConventionalControllers.Create</c> inside <c>Configure&lt;&gt;</c> and services behind
    /// hand-written controllers are.
    /// <para>
    /// So when the resolved instance is not a proxy, or its implementation type is in <c>DynamicProxyIgnoreTypes</c>, the invoker
    /// runs the same ABP services the interceptors would run, against the implementation method, before calling it:
    /// global feature check, <see cref="IMethodInvocationAuthorizationService"/>, <see cref="IMethodInvocationFeatureCheckerService"/>, <see cref="IMethodInvocationValidator"/> (only for
    /// <see cref="IValidationEnabled"/> services, as ABP's registrar), and an audit log action like <c>AuditingInterceptor</c>
    /// when an audit scope is current. Errors therefore keep ABP's own types (AbpAuthorizationException,
    /// AbpValidationException). The unit of work is the invoker's own anyway.
    /// Not replicated: a new audit scope when none is current (AuditingInterceptor would open and save one), entity
    /// change tracking / entity history interceptors, and any interceptor an application adds itself.
    /// </para>
    /// </summary>
    protected virtual async Task<object?> InvokeUnproxiedAsync(
        IServiceProvider services,
        AgentToolDefinition definition,
        object service,
        object?[] parameterValues)
    {
        var method = definition.ImplementationMethod;
        var auditingOptions = services.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
        var auditingHelper = services.GetRequiredService<IAuditingHelper>();
        var auditLog = services.GetRequiredService<IAuditingManager>().Current?.Log;
        var audit = auditLog != null
                    && auditingOptions.IsEnabled
                    && !AbpCrossCuttingConcerns.IsApplied(service, AbpCrossCuttingConcerns.Auditing)
                    && auditingHelper.ShouldSaveAudit(method, ignoreIntegrationServiceAttribute: auditingOptions.IsEnabledForIntegrationServices);
        var auditAction = audit && !auditingOptions.DisableLogActionInfo
            ? auditingHelper.CreateAuditLogAction(auditLog!, definition.ImplementationType, method, parameterValues)
            : null;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            if (service is IGlobalFeatureCheckingEnabled
                && !AbpCrossCuttingConcerns.IsApplied(service, AbpCrossCuttingConcerns.GlobalFeatureChecking)
                && !GlobalFeatureHelper.IsGlobalFeatureEnabled(definition.ImplementationType, out var globalFeature))
            {
                throw new AbpGlobalFeatureNotEnabledException(code: AbpGlobalFeatureErrorCodes.GlobalFeatureIsNotEnabled)
                    .WithData("ServiceName", definition.ImplementationType.FullName!)
                    .WithData("GlobalFeatureName", globalFeature!.Name!);
            }

            await services.GetRequiredService<IMethodInvocationAuthorizationService>()
                .CheckAsync(new MethodInvocationAuthorizationContext(method));

            if (!AbpCrossCuttingConcerns.IsApplied(service, AbpCrossCuttingConcerns.FeatureChecking))
            {
                await services.GetRequiredService<IMethodInvocationFeatureCheckerService>()
                    .CheckAsync(new MethodInvocationFeatureCheckerContext(method));
            }

            if (service is IValidationEnabled)
            {
                await services.GetRequiredService<IMethodInvocationValidator>()
                    .ValidateAsync(new MethodInvocationValidationContext(service, method, parameterValues));
            }

            return await InvokeMethodAsync(definition, service, parameterValues);
        }
        catch (Exception ex) when (audit)
        {
            auditLog!.Exceptions.Add(ex);
            throw;
        }
        finally
        {
            if (auditAction != null)
            {
                auditAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditLog!.Actions.Add(auditAction);
            }
        }
    }

    /// <summary>Calls the interface method on the resolved instance (the proxy when there is one) and awaits it.</summary>
    protected virtual async Task<object?> InvokeMethodAsync(AgentToolDefinition definition, object service, object?[] parameterValues)
    {
        var method = definition.InterfaceMethod;
        var returnValue = method.Invoke(service, BindingFlags.DoNotWrapExceptions, null, parameterValues, null);
        if (returnValue is not Task task)
        {
            // ValueTask returns are not awaited; application services return Task.
            return returnValue;
        }

        await task;
        return method.ReturnType.IsGenericType ? method.ReturnType.GetProperty(nameof(Task<object>.Result))!.GetValue(task) : null;
    }

    /// <summary>
    /// Binds by parameter name (exact, then case-insensitive): JsonElement/JsonNode values are deserialized with ABP's
    /// System.Text.Json options, other values are used as they are when assignable, else converted through JSON.
    /// Missing parameters with a default get it. Missing required ones and values that do not convert to the parameter type
    /// fail as one AbpValidationException naming the parameters, so the model sees which field is wrong.
    /// </summary>
    protected virtual object?[] BindArguments(
        AgentToolDefinition definition,
        IDictionary<string, object?> arguments,
        JsonSerializerOptions jsonOptions,
        CancellationToken cancellationToken)
    {
        var parameters = definition.InterfaceMethod.GetParameters();
        var values = new object?[parameters.Length];
        var errors = new List<ValidationResult>();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var type = parameter.ParameterType;

            if (type == typeof(CancellationToken))
            {
                values[i] = cancellationToken;
            }
            else if (TryGetArgument(arguments, parameter.Name!, out var value))
            {
                try
                {
                    values[i] = ConvertArgument(value, type, jsonOptions);
                }
                catch (Exception ex) when (ex is JsonException or FormatException or InvalidCastException or NotSupportedException
                                               or OverflowException or ArgumentException or InvalidOperationException)
                {
                    errors.Add(new ValidationResult(
                        $"The {parameter.Name} argument is not a valid {type.Name}: {ex.Message}", [parameter.Name!]));
                }
            }
            else if (parameter.HasDefaultValue)
            {
                values[i] = parameter.DefaultValue;
            }
            else
            {
                if (definition.Parameters.FirstOrDefault(p => p.Name == parameter.Name)?.IsRequired ?? true)
                {
                    errors.Add(new ValidationResult($"The {parameter.Name} field is required.", [parameter.Name!]));
                }

                values[i] = type.IsValueType ? Activator.CreateInstance(type) : null;
            }
        }

        if (errors.Count > 0)
        {
            throw new AbpValidationException("Invalid tool arguments: " + errors.SelectMany(e => e.MemberNames).JoinAsString(", ") + ".", errors);
        }

        return values;
    }

    protected static bool TryGetArgument(IDictionary<string, object?> arguments, string name, out object? value)
    {
        if (arguments.TryGetValue(name, out value))
        {
            return true;
        }

        foreach (var argument in arguments)
        {
            if (string.Equals(argument.Key, name, StringComparison.OrdinalIgnoreCase))
            {
                value = argument.Value;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// A copy of ABP's options (made once) for tool arguments, results and the logged arguments: enums are written as their
    /// names, as the schema lists them; names and numbers are both read. ABP's own options (HTTP API) are unchanged.
    /// </summary>
    private static JsonSerializerOptions GetToolJsonOptions(JsonSerializerOptions abpOptions)
    {
        return ToolJsonOptions.GetValue(abpOptions, source =>
        {
            var options = new JsonSerializerOptions(source);
            // First, so it wins over ABP's AbpStringToEnumFactory, which writes numbers.
            options.Converters.Insert(0, new JsonStringEnumConverter());
            return options;
        });
    }

    protected static object? ConvertArgument(object? value, Type type, JsonSerializerOptions jsonOptions)
    {
        return value switch
        {
            null => null,
            JsonElement element => element.Deserialize(type, jsonOptions),
            JsonNode node => node.Deserialize(type, jsonOptions),
            _ when type.IsInstanceOfType(value) => value,
            _ => JsonSerializer.SerializeToElement(value, value.GetType(), jsonOptions).Deserialize(type, jsonOptions)
        };
    }

    /// <summary>
    /// The message the model sees, from ABP's <see cref="IExceptionToErrorInfoConverter"/> (as an HTTP client would see it:
    /// localized, internal errors hidden unless SendExceptionsDetailsToClients; never a stack trace), plus validation
    /// field messages and the error code.
    /// </summary>
    protected virtual string FormatErrorMessage(IServiceProvider services, Exception exception)
    {
        var errorInfo = services.GetRequiredService<IExceptionToErrorInfoConverter>()
            .Convert(exception, options => options.SendStackTraceToClients = false);

        var message = errorInfo.Message ?? exception.Message;

        // A business exception without a localized code would come out as the generic "internal error" text; its own
        // message is written by the application for this case and tells the model what went wrong.
        if (exception is IBusinessException && !exception.Message.IsNullOrWhiteSpace()
            && message == services.GetRequiredService<IStringLocalizer<AbpExceptionHandlingResource>>()["InternalServerErrorMessage"])
        {
            message = exception.Message;
        }

        if (exception is IHasValidationErrors { ValidationErrors.Count: > 0 } validation)
        {
            message += " Validation errors: " + validation.ValidationErrors
                .Select(e => e.MemberNames.Any() ? $"[{e.MemberNames.JoinAsString(",")}] {e.ErrorMessage}" : e.ErrorMessage ?? "")
                .JoinAsString("; ");
        }
        else if (!errorInfo.Details.IsNullOrWhiteSpace() && errorInfo.Details != message)
        {
            message += " Details: " + errorInfo.Details;
        }

        if (exception is IHasErrorCode { Code: not null } withCode)
        {
            message += $" (code: {withCode.Code})";
        }

        return message;
    }

    /// <summary>Arguments as compact JSON with values of password-like names (nested too, also inside JSON strings) replaced by "***".</summary>
    protected virtual string RedactArguments(IDictionary<string, object?> arguments, JsonSerializerOptions jsonOptions)
    {
        try
        {
            return AgentToolArgumentRedactor.ToRedactedJson(arguments, jsonOptions, IsSensitiveArgumentName);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Agent tool arguments could not be serialized for the log.");
            return "\"(arguments could not be serialized)\"";
        }
    }

    protected virtual bool IsSensitiveArgumentName(string name) =>
        AgentToolArgumentRedactor.IsSensitiveName(name, Options?.Value.AdditionalSensitiveArgumentNames);

    /// <summary>The name with control characters and line separators replaced, at most <see cref="MaxShownNameLength"/> long; tool names stay as they are.</summary>
    protected static string GetShownName(string name)
    {
        if (name.Length > MaxShownNameLength)
        {
            // Not cut inside a surrogate pair.
            name = name[..(char.IsHighSurrogate(name[MaxShownNameLength - 1]) ? MaxShownNameLength - 1 : MaxShownNameLength)];
        }

        return string.Concat(name.Select(c => char.IsControl(c) || c is '\u2028' or '\u2029' ? '?' : c));
    }

    /// <summary>The visible trace in the request's audit log: which agent tool, which client.</summary>
    protected virtual void AddAuditComment(IServiceProvider services, string toolName, AgentToolContext context)
    {
        services.GetRequiredService<IAuditingManager>().Current?.Log.Comments.Add(
            $"Agent tool {toolName} (client: {context.ClientId ?? "none"}{(context.ClientName.IsNullOrWhiteSpace() ? "" : "; " + context.ClientName.Truncate(100))})");
    }

    protected virtual async Task NotifyObserversAsync(IServiceProvider services, AgentToolCallRecord record)
    {
        foreach (var observer in services.GetServices<IAgentToolCallObserver>())
        {
            try
            {
                await observer.OnCallAsync(record);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Agent tool call observer {Observer} failed for {ToolName}.", observer.GetType().FullName, record.ToolName);
            }
        }
    }

    private static BusinessException Refusal(string code, string message, string toolName)
    {
        return new BusinessException(code, message).WithData("ToolName", toolName);
    }
}
