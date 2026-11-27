using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Builds the tool catalogue once (lazily, thread-safe) from ABP's API description model plus the ApiExplorer
/// <see cref="ControllerActionDescriptor"/> of each action. Included actions that cannot be tools are listed in <see cref="Skipped"/>.
/// The build validates every tool's input schema (<see cref="AgentToolSchemaValidator"/>) and warns about full-replace
/// PUT/PATCH inputs; the module forces it at application initialization.
/// </summary>
public class AgentToolCatalog : IAgentToolCatalog
{
    public const int MaxNameLength = 64;

    private static readonly Regex InvalidNameCharacters = new("[^A-Za-z0-9_-]", RegexOptions.Compiled);
    protected const string ServiceInterfaceReasonPrefix = "service interface";

    private static readonly Regex ValidName = new("^[A-Za-z0-9_-]{1,64}$", RegexOptions.Compiled);

    private readonly AbpAgentToolOptions _options;
    private readonly IApiDescriptionModelProvider _apiModelProvider;
    private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionProvider;
    private readonly Func<MethodInfo, string> _actionNameGenerator;
    private readonly HashSet<Type> _ignoredInterfaces;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AgentToolCatalog> _logger;
    private readonly Lazy<Catalog> _catalog;

    public AgentToolCatalog(
        IOptions<AbpAgentToolOptions> options,
        IApiDescriptionModelProvider apiModelProvider,
        IApiDescriptionGroupCollectionProvider apiDescriptionProvider,
        IOptions<AspNetCoreApiDescriptionModelProviderOptions> modelProviderOptions,
        IOptions<AbpApiDescriptionModelOptions> modelOptions,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AgentToolCatalog> logger)
    {
        _options = options.Value;
        _apiModelProvider = apiModelProvider;
        _apiDescriptionProvider = apiDescriptionProvider;
        _actionNameGenerator = modelProviderOptions.Value.ActionNameGenerator;
        _ignoredInterfaces = modelOptions.Value.IgnoredInterfaces;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _catalog = new Lazy<Catalog>(Build, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public IReadOnlyList<AgentToolDefinition> Tools => _catalog.Value.Tools;

    public IReadOnlyList<SkippedAgentToolAction> Skipped => _catalog.Value.Skipped;

    public AgentToolDefinition? Find(string name) => _catalog.Value.ByName.GetValueOrDefault(name);

    protected virtual Catalog Build()
    {
        if (!_options.HasAnyInclusion)
        {
            return new Catalog([], []);
        }

        // The model is async only for XML docs (cached file reads); built once, so RunSync is acceptable.
        var model = AsyncHelper.RunSync(() => _apiModelProvider.CreateApiModelAsync(
            new ApplicationApiDescriptionModelRequestDto { IncludeTypes = false, IncludeDescriptions = true }));

        var apiDescriptions = new Dictionary<(string, string), ApiDescription>();
        foreach (var api in _apiDescriptionProvider.ApiDescriptionGroups.Items.SelectMany(g => g.Items))
        {
            if (api.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                // Same key and first-wins rule as the model (controller type, ActionNameGenerator).
                apiDescriptions.TryAdd((descriptor.ControllerTypeInfo.FullName!, _actionNameGenerator(descriptor.MethodInfo)), api);
            }
        }

        var included = new List<Candidate>();
        var skipped = new List<SkippedAgentToolAction>();
        var seen = new HashSet<(string, string)>();

        // Implementation type per service interface, resolved once in one scope (see FindImplementationType).
        using var scope = _serviceScopeFactory.CreateScope();
        var implementations = new Dictionary<Type, (Type? Type, string? Error)>();

        foreach (var module in model.Modules.Values)
        {
            foreach (var controller in module.Controllers.Values)
            {
                foreach (var action in controller.Actions.Values)
                {
                    // seen: the same controller can appear once per API version.
                    if (!seen.Add((controller.Type, action.UniqueName)) ||
                        !apiDescriptions.TryGetValue((controller.Type, action.UniqueName), out var api))
                    {
                        continue;
                    }

                    var descriptor = (ControllerActionDescriptor)api.ActionDescriptor;
                    var controllerType = descriptor.ControllerTypeInfo.AsType();
                    var (serviceInterface, interfaceMethod) = FindServiceInterface(controllerType, descriptor.MethodInfo);

                    if (!IsIncluded(module, serviceInterface ?? controllerType))
                    {
                        continue;
                    }

                    Type? implementationType = null;
                    MethodInfo? implementationMethod = null;
                    string? implementationError = null;
                    if (serviceInterface != null && interfaceMethod != null)
                    {
                        if (!implementations.TryGetValue(serviceInterface, out var implementation))
                        {
                            implementation.Type = FindImplementationType(scope.ServiceProvider, serviceInterface, out implementation.Error);
                            implementations[serviceInterface] = implementation;
                        }

                        (implementationType, implementationError) = implementation;
                        implementationMethod = implementationType == null ? null : FindImplementationMethod(implementationType, interfaceMethod);
                    }

                    var reason = GetSkipReason(controller, action, api, descriptor, serviceInterface, interfaceMethod,
                        implementationType, implementationMethod, implementationError);
                    if (reason != null)
                    {
                        skipped.Add(new SkippedAgentToolAction(module.RemoteServiceName, module.RootPath,
                            controller.ControllerName, controller.Type, action.Name, reason));
                        continue;
                    }

                    included.Add(new Candidate(module, controller, action, descriptor.MethodInfo, serviceInterface!, interfaceMethod!,
                        implementationType!, implementationMethod!, controllerType));
                }
            }
        }

        foreach (var skip in skipped)
        {
            // A service interface DI cannot give us (or gives a controller for) is a registration problem: warn.
            _logger.Log(skip.Reason.StartsWith(ServiceInterfaceReasonPrefix) ? LogLevel.Warning : LogLevel.Information,
                "Agent tool skipped: {ControllerType}.{Action} ({RemoteService}): {Reason}",
                skip.ControllerType, skip.ActionName, skip.RemoteServiceName, skip.Reason);
        }

        if (included.Count > _options.MaxTools)
        {
            throw new AbpException(
                $"Agent tools: {included.Count} actions are included as tools, but AbpAgentToolOptions.MaxTools is {_options.MaxTools}. " +
                "Include fewer remote services, root paths or service types (or exclude some with ExcludedServiceTypes or [DisableAgentTool]), " +
                "or raise AbpAgentToolOptions.MaxTools.");
        }

        // Names first, so leaving a tool out never renames another (overload suffixes stay as they are).
        var tools = new List<AgentToolDefinition>();
        var invalid = new List<string>();
        foreach (var (candidate, name) in AssignNames(included))
        {
            var tool = CreateDefinition(candidate, name);
            JsonElement schema;
            string? error;
            try
            {
                schema = AgentToolJsonSchema.Get(tool);
                error = AgentToolSchemaValidator.Validate(schema);
            }
            catch (Exception ex)
            {
                schema = default;
                error = "the schema could not be created: " + ex.Message;
            }

            if (error != null)
            {
                invalid.Add($"'{name}' ({candidate.Source}): {error}");
                _logger.LogError("Agent tool {Tool} ({Source}) left out: its input schema is invalid: {Reason}", name, candidate.Source, error);
                skipped.Add(new SkippedAgentToolAction(candidate.Module.RemoteServiceName, candidate.Module.RootPath,
                    candidate.Controller.ControllerName, candidate.Controller.Type, candidate.Action.Name, "invalid input schema: " + error));
                continue;
            }

            if (FindFullReplaceInput(tool, schema) is { } input)
            {
                _logger.LogWarning(
                    "Agent tool {Tool} ({HttpMethod} {Url}) replaces the whole entity: its input '{Parameter}' ({InputType}) has no optional " +
                    "(nullable, not required) property besides the id and the concurrency stamp, so an agent that changes one field clears the others. " +
                    "Prefer a patch-style input DTO whose nullable properties mean \"leave unchanged\".",
                    name, tool.HttpMethod, tool.Url, input.Name, input.Type.FullName);
            }

            tools.Add(tool);
        }

        if (invalid.Count > 0 && _options.FailOnInvalidSchema)
        {
            throw new AbpException("Agent tools: invalid tool input schemas (AbpAgentToolOptions.FailOnInvalidSchema is true). " +
                                   invalid.JoinAsString(" ") + " Fix the input DTO, exclude the action (ExcludedServiceTypes or [DisableAgentTool]), " +
                                   "or set FailOnInvalidSchema to false to leave such tools out with an error log.");
        }

        tools.Sort((x, y) => StringComparer.Ordinal.Compare(x.Name, y.Name));

        _logger.LogInformation("Agent tool catalogue: {ToolCount} tools, {SkippedCount} included actions skipped.", tools.Count, skipped.Count);

        return new Catalog(tools, skipped);
    }

    protected virtual bool IsIncluded(ModuleApiDescriptionModel module, Type serviceType)
    {
        return _options.IncludedRemoteServiceNames.Contains(module.RemoteServiceName)
               || _options.IncludedRootPaths.Contains(module.RootPath)
               || _options.IncludedServiceTypes.Any(predicate => predicate(serviceType));
    }

    protected virtual string? GetSkipReason(
        ControllerApiDescriptionModel controller,
        ActionApiDescriptionModel action,
        ApiDescription api,
        ControllerActionDescriptor descriptor,
        Type? serviceInterface,
        MethodInfo? interfaceMethod,
        Type? implementationType,
        MethodInfo? implementationMethod,
        string? implementationError)
    {
        if (_options.ExcludedServiceTypes.Any(predicate => predicate(serviceInterface ?? descriptor.ControllerTypeInfo.AsType())))
        {
            return "excluded by AbpAgentToolOptions.ExcludedServiceTypes";
        }

        if (serviceInterface == null || interfaceMethod == null)
        {
            return "no service interface: the controller implements no interface that declares this action";
        }

        if (controller.IsIntegrationService || IntegrationServiceAttribute.IsDefinedOrInherited(serviceInterface))
        {
            return "integration service";
        }

        if (action.AllowAnonymous == true || interfaceMethod.IsDefined(typeof(AllowAnonymousAttribute), true))
        {
            return "[AllowAnonymous]";
        }

        if (implementationType == null || implementationMethod == null)
        {
            return implementationError ?? "service interface not registered";
        }

        // Anonymous at any layer is never a tool: also the implementation class behind a hand-written controller, whose
        // [AllowAnonymous] ABP's authorization interceptor would honor on an in-process call.
        if (implementationMethod.IsDefined(typeof(AllowAnonymousAttribute), true)
            || implementationType.IsDefined(typeof(AllowAnonymousAttribute), true))
        {
            return "[AllowAnonymous]";
        }

        if (typeof(ControllerBase).IsAssignableFrom(implementationType))
        {
            return ServiceInterfaceReasonPrefix + " resolves to a controller (" + implementationType.FullName + "); controllers are never invoked";
        }

        if (descriptor.MethodInfo.IsDefined(typeof(DisableAgentToolAttribute), true)
            || interfaceMethod.IsDefined(typeof(DisableAgentToolAttribute), true)
            || descriptor.ControllerTypeInfo.IsDefined(typeof(DisableAgentToolAttribute), true)
            || serviceInterface.IsDefined(typeof(DisableAgentToolAttribute), true)
            || interfaceMethod.DeclaringType!.IsDefined(typeof(DisableAgentToolAttribute), true)
            || implementationMethod.IsDefined(typeof(DisableAgentToolAttribute), true)
            || implementationType.IsDefined(typeof(DisableAgentToolAttribute), true))
        {
            return "[DisableAgentTool]";
        }

        if (api.ParameterDescriptions.Any(p => p.Source == BindingSource.Form || p.Source == BindingSource.FormFile)
            || interfaceMethod.GetParameters().Any(p => IsNonJson(p.ParameterType)
                || (p.ParameterType.IsClass && p.ParameterType != typeof(string) && p.ParameterType.GetProperties().Any(pr => IsNonJson(pr.PropertyType)))))
        {
            return "non-JSON input (file upload, stream or form binding)";
        }

        // ApiExplorer lists the header members of a query DTO, but no services; it also lists the properties of the controller,
        // which are no arguments of the tool.
        if (api.ParameterDescriptions.Any(p => p.Source == BindingSource.Header && p.ParameterDescriptor is ControllerParameterDescriptor)
            || descriptor.Parameters.Any(p => IsFromServicesOrHeader(p.BindingInfo))
            || implementationMethod.GetParameters().Concat(interfaceMethod.GetParameters())
                .Any(p => IsFromServicesOrHeader(BindingInfo.GetBindingInfo(p.GetCustomAttributes(true)))))
        {
            return "input from services or a header ([FromServices], [FromKeyedServices] or [FromHeader])";
        }

        var returnType = AsyncHelper.UnwrapTask(interfaceMethod.ReturnType);
        if (IsNonJson(returnType)
            || typeof(IActionResult).IsAssignableFrom(returnType)
            || typeof(IConvertToActionResult).IsAssignableFrom(returnType))
        {
            return "non-JSON return (stream, file or action result)";
        }

        return null;
    }

    /// <summary>
    /// The interface declaring the action method (via the controller's interface maps) and, as service interface,
    /// the most derived interface of the controller that includes it (IBookAppService rather than ICrudAppService&lt;...&gt;),
    /// which is what DI resolves.
    /// </summary>
    protected virtual (Type? ServiceInterface, MethodInfo? InterfaceMethod) FindServiceInterface(Type controllerType, MethodInfo method)
    {
        var interfaces = controllerType.GetInterfaces().Where(i => !_ignoredInterfaces.Contains(i)).ToArray();
        foreach (var declaring in interfaces)
        {
            InterfaceMapping map;
            try
            {
                map = controllerType.GetInterfaceMap(declaring);
            }
            catch (ArgumentException)
            {
                continue;
            }

            var index = Array.FindIndex(map.TargetMethods, m => m.MethodHandle == method.MethodHandle && m.DeclaringType == method.DeclaringType);
            if (index < 0)
            {
                continue;
            }

            var candidates = interfaces.Where(declaring.IsAssignableFrom).ToArray();
            var service = candidates
                .Where(c => !candidates.Any(other => other != c && c.IsAssignableFrom(other)))
                .OrderBy(c => c.FullName, StringComparer.Ordinal)
                .First();

            return (service, map.InterfaceMethods[index]);
        }

        return (null, null);
    }

    /// <summary>
    /// The class DI resolves for the service interface (unproxied): its attributes count like the controller's, and the
    /// permission pre-check reads [Authorize] from it like ABP's interceptor. Resolved once per interface at build time.
    /// </summary>
    protected virtual Type? FindImplementationType(IServiceProvider services, Type serviceInterface, out string? error)
    {
        error = null;
        try
        {
            var instance = services.GetService(serviceInterface);
            if (instance != null)
            {
                return ProxyHelper.GetUnProxiedType(instance);
            }

            error = "service interface not registered";
        }
        catch (Exception ex)
        {
            error = "service interface could not be resolved: " + ex.Message;
        }

        return null;
    }

    protected static MethodInfo? FindImplementationMethod(Type implementationType, MethodInfo interfaceMethod)
    {
        if (!interfaceMethod.DeclaringType!.IsAssignableFrom(implementationType))
        {
            return null;
        }

        var map = implementationType.GetInterfaceMap(interfaceMethod.DeclaringType);
        var index = Array.FindIndex(map.InterfaceMethods, m => m.MethodHandle == interfaceMethod.MethodHandle);
        return index < 0 ? null : map.TargetMethods[index];
    }

    protected virtual List<(Candidate Candidate, string Name)> AssignNames(List<Candidate> candidates)
    {
        var named = new List<(Candidate Candidate, string Name)>();

        foreach (var candidate in candidates.Where(c => c.Attribute.Name != null))
        {
            if (!ValidName.IsMatch(candidate.Attribute.Name!))
            {
                throw new AbpException(
                    $"Agent tools: [AgentTool(Name = \"{candidate.Attribute.Name}\")] on {candidate.Source} is not a valid tool name. " +
                    $"Use only A-Z, a-z, 0-9, '_' and '-', at most {MaxNameLength} characters.");
            }

            named.Add((candidate, candidate.Attribute.Name!));
        }

        // Overloads (same derived name in one controller) get _2, _3, ... in interface declaration order.
        var overloadGroups = candidates
            .Where(c => c.Attribute.Name == null)
            .GroupBy(c => (c.Controller.Type, DeriveName(c.Controller.ControllerName, c.Action.Name)));

        foreach (var group in overloadGroups)
        {
            var ordered = group
                .OrderBy(c => c.InterfaceMethod.DeclaringType!.FullName, StringComparer.Ordinal)
                .ThenBy(c => c.InterfaceMethod.MetadataToken)
                .ToList();

            for (var i = 0; i < ordered.Count; i++)
            {
                var suffix = i == 0 ? "" : "_" + (i + 1);
                var baseName = group.Key.Item2;
                named.Add((ordered[i], baseName.Left(Math.Min(baseName.Length, MaxNameLength - suffix.Length)) + suffix));
            }
        }

        // Without case, as policies match tool names.
        var duplicates = named.GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase).Where(g => g.Count() > 1).ToList();
        if (duplicates.Count > 0)
        {
            throw new AbpException("Agent tools: duplicate tool names (case is ignored). " + duplicates
                .Select(g => g.Select(x => $"'{x.Name}' ({x.Candidate.Source})").JoinAsString(" and ") + ".")
                .JoinAsString(" ") + " Rename one with [AgentTool(Name = ...)] or exclude it.");
        }

        return named;
    }

    protected virtual AgentToolDefinition CreateDefinition(Candidate candidate, string name)
    {
        var attribute = candidate.Attribute;
        var action = candidate.Action;
        var httpMethod = (action.HttpMethod ?? "POST").ToUpperInvariant();

        var (readOnly, destructive, idempotent) = httpMethod switch
        {
            "GET" => (true, false, true),
            "PUT" => (false, false, true),
            "DELETE" => (false, true, true),
            _ => (false, false, false) // POST, PATCH and anything else
        };

        // An explicit ReadOnly / Destructive implies the opposite of the other unless that one is set too.
        var attrReadOnly = attribute.GetReadOnly();
        var attrDestructive = attribute.GetDestructive();
        readOnly = attrReadOnly ?? (attrDestructive == true ? false : readOnly);
        destructive = attrDestructive ?? (attrReadOnly == true ? false : destructive);
        idempotent = attribute.GetIdempotent() ?? (attrReadOnly == true || idempotent);
        var kind = destructive ? AgentToolKind.Destructive : readOnly ? AgentToolKind.Read : AgentToolKind.Write;

        var description = attribute.Description
                          ?? candidate.InterfaceMethod.GetCustomAttribute<DescriptionAttribute>()?.Description
                          ?? candidate.Method.GetCustomAttribute<DescriptionAttribute>()?.Description
                          ?? candidate.ImplementationMethod.GetCustomAttribute<DescriptionAttribute>()?.Description
                          ?? action.Summary
                          ?? $"Calls {candidate.ServiceInterface.Name.RemovePreFix("I")}.{action.Name.RemovePostFix("Async")} ({httpMethod} {action.Url}).";

        return new AgentToolDefinition
        {
            Name = name,
            Description = description,
            Kind = kind,
            ReadOnly = readOnly,
            Destructive = destructive,
            Idempotent = idempotent,
            RequiresApprovalByDefault = attribute.GetRequiresApproval() ?? kind != AgentToolKind.Read,
            ServiceInterface = candidate.ServiceInterface,
            InterfaceMethod = candidate.InterfaceMethod,
            ImplementationType = candidate.ImplementationType,
            ImplementationMethod = candidate.ImplementationMethod,
            ControllerActionMethod = candidate.ControllerType == candidate.ImplementationType ? null : candidate.Method,
            ControllerType = candidate.ControllerType == candidate.ImplementationType ? null : candidate.ControllerType,
            HttpMethod = httpMethod,
            Url = action.Url,
            RemoteServiceName = candidate.Module.RemoteServiceName,
            ModuleRootPath = candidate.Module.RootPath,
            ControllerName = candidate.Controller.ControllerName,
            ActionName = action.Name,
            Parameters = CreateParameters(candidate),
            RequireNonNullableValueTypes = _options.RequireNonNullableValueTypes
        };
    }

    protected virtual List<AgentToolParameter> CreateParameters(Candidate candidate)
    {
        // ParametersOnMethod lists the controller method's parameters; the interface method has the same positions.
        var parameters = new List<AgentToolParameter>();
        var interfaceParameters = candidate.InterfaceMethod.GetParameters();
        for (var i = 0; i < interfaceParameters.Length; i++)
        {
            var parameter = interfaceParameters[i];
            if (parameter.ParameterType == typeof(CancellationToken))
            {
                continue;
            }

            var model = i < candidate.Action.ParametersOnMethod.Count ? candidate.Action.ParametersOnMethod[i] : null;
            parameters.Add(new AgentToolParameter(
                parameter.Name!,
                parameter.ParameterType,
                !(model?.IsOptional ?? parameter.IsOptional),
                parameter.GetCustomAttribute<DescriptionAttribute>()?.Description ?? model?.Description ?? model?.Summary));
        }

        return parameters;
    }

    /// <summary>
    /// For PUT and PATCH: the first object input (the body DTO) without an optional property besides id and concurrencyStamp, or null. Optional
    /// is the schema's rule: not in the object's <c>required</c> list and nullable (<c>T?</c>, or a reference type in code
    /// without nullable annotations).
    /// </summary>
    private static AgentToolParameter? FindFullReplaceInput(AgentToolDefinition tool, JsonElement schema)
    {
        if (tool.HttpMethod is not ("PUT" or "PATCH") || !schema.TryGetProperty("properties", out var properties))
        {
            return null;
        }

        foreach (var parameter in tool.Parameters)
        {
            var typeInfo = AIJsonUtilities.DefaultOptions.GetTypeInfo(parameter.Type);
            if (typeInfo.Kind != JsonTypeInfoKind.Object || !properties.TryGetProperty(parameter.Name, out var input))
            {
                continue;
            }

            var required = input.TryGetProperty("required", out var list) ? list.EnumerateArray().Select(e => e.GetString()).ToHashSet() : [];
            // The id and IHasConcurrencyStamp's stamp say which row and version, not what to change; extraProperties has a protected setter.
            if (!typeInfo.Properties.Any(p => p.Set != null && p.IsSetNullable && !required.Contains(p.Name)
                                              && !string.Equals(p.Name, "id", StringComparison.OrdinalIgnoreCase)
                                              && !string.Equals(p.Name, "concurrencyStamp", StringComparison.OrdinalIgnoreCase)))
            {
                return parameter;
            }
        }

        return null;
    }

    protected static string DeriveName(string controllerName, string actionName)
    {
        return InvalidNameCharacters.Replace($"{controllerName}_{actionName.RemovePostFix("Async")}", "_");
    }

    private static bool IsFromServicesOrHeader(BindingInfo? binding)
    {
        return binding?.BindingSource == BindingSource.Services || binding?.BindingSource == BindingSource.Header;
    }

    protected static bool IsNonJson(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (typeof(Stream).IsAssignableFrom(type)
            || typeof(IRemoteStreamContent).IsAssignableFrom(type)
            || typeof(IFormFile).IsAssignableFrom(type)
            || typeof(IFormFileCollection).IsAssignableFrom(type))
        {
            return true;
        }

        return TypeHelper.IsEnumerable(type, out var itemType, includePrimitives: false)
               && itemType != null && itemType != type && itemType != typeof(object)
               && (typeof(Stream).IsAssignableFrom(itemType)
                   || typeof(IRemoteStreamContent).IsAssignableFrom(itemType)
                   || typeof(IFormFile).IsAssignableFrom(itemType));
    }

    protected record Catalog(IReadOnlyList<AgentToolDefinition> Tools, IReadOnlyList<SkippedAgentToolAction> Skipped)
    {
        public Dictionary<string, AgentToolDefinition> ByName { get; } = Tools.ToDictionary(t => t.Name, StringComparer.Ordinal);
    }

    protected record Candidate(
        ModuleApiDescriptionModel Module,
        ControllerApiDescriptionModel Controller,
        ActionApiDescriptionModel Action,
        MethodInfo Method,
        Type ServiceInterface,
        MethodInfo InterfaceMethod,
        Type ImplementationType,
        MethodInfo ImplementationMethod,
        Type ControllerType)
    {
        /// <summary>
        /// [AgentTool] per property: the implementation class method (behind the interface in DI) over the controller method
        /// over the interface method. For conventional controllers the first two are the same method.
        /// </summary>
        public AgentToolAttribute Attribute { get; } = Merge(
            ImplementationMethod.GetCustomAttribute<AgentToolAttribute>(),
            Merge(Method.GetCustomAttribute<AgentToolAttribute>(), InterfaceMethod.GetCustomAttribute<AgentToolAttribute>()));

        public string Source => $"{ServiceInterface.Name}.{InterfaceMethod.Name} ({Action.HttpMethod} {Action.Url})";

        private static AgentToolAttribute Merge(AgentToolAttribute? implementation, AgentToolAttribute? contract)
        {
            var merged = new AgentToolAttribute
            {
                Name = implementation?.Name ?? contract?.Name,
                Description = implementation?.Description ?? contract?.Description
            };

            if ((implementation?.GetRequiresApproval() ?? contract?.GetRequiresApproval()) is { } requiresApproval)
            {
                merged.RequiresApproval = requiresApproval;
            }

            if ((implementation?.GetReadOnly() ?? contract?.GetReadOnly()) is { } readOnly)
            {
                merged.ReadOnly = readOnly;
            }

            if ((implementation?.GetDestructive() ?? contract?.GetDestructive()) is { } destructive)
            {
                merged.Destructive = destructive;
            }

            if ((implementation?.GetIdempotent() ?? contract?.GetIdempotent()) is { } idempotent)
            {
                merged.Idempotent = idempotent;
            }

            return merged;
        }
    }
}
