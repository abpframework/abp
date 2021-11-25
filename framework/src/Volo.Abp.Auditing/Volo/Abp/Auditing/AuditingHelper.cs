using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Users;

namespace Volo.Abp.Auditing;

public class AuditingHelper : IAuditingHelper, ITransientDependency
{
    protected ILogger<AuditingHelper> Logger { get; }
    protected IAuditingStore AuditingStore { get; }
    protected ICurrentUser CurrentUser { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected ICurrentClient CurrentClient { get; }
    protected IClock Clock { get; }
    protected AbpAuditingOptions Options;
    protected IAuditSerializer AuditSerializer;
    protected IServiceProvider ServiceProvider;
    protected ICorrelationIdProvider CorrelationIdProvider { get; }

    public AuditingHelper(
        IAuditSerializer auditSerializer,
        IOptions<AbpAuditingOptions> options,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        ICurrentClient currentClient,
        IClock clock,
        IAuditingStore auditingStore,
        ILogger<AuditingHelper> logger,
        IServiceProvider serviceProvider,
        ICorrelationIdProvider correlationIdProvider)
    {
        Options = options.Value;
        AuditSerializer = auditSerializer;
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
        CurrentClient = currentClient;
        Clock = clock;
        AuditingStore = auditingStore;

        Logger = logger;
        ServiceProvider = serviceProvider;
        CorrelationIdProvider = correlationIdProvider;
    }

    public virtual bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false)
    {
        if (methodInfo == null)
        {
            return false;
        }

        if (!methodInfo.IsPublic)
        {
            return false;
        }

        if (methodInfo.IsDefined(typeof(AuditedAttribute), true))
        {
            return true;
        }

        if (methodInfo.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            return false;
        }

        var classType = methodInfo.DeclaringType;
        if (classType != null)
        {
            var shouldAudit = AuditingInterceptorRegistrar.ShouldAuditTypeByDefaultOrNull(classType);
            if (shouldAudit != null)
            {
                return shouldAudit.Value;
            }
        }

        return defaultValue;
    }

    public virtual bool IsEntityHistoryEnabled(Type entityType, bool defaultValue = false)
    {
        if (!entityType.IsPublic)
        {
            return false;
        }

        if (Options.IgnoredTypes.Any(t => t.IsAssignableFrom(entityType)))
        {
            return false;
        }

        if (entityType.IsDefined(typeof(AuditedAttribute), true))
        {
            return true;
        }

        foreach (var propertyInfo in entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (propertyInfo.IsDefined(typeof(AuditedAttribute)))
            {
                return true;
            }
        }

        if (entityType.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            return false;
        }

        if (Options.EntityHistorySelectors.Any(selector => selector.Predicate(entityType)))
        {
            return true;
        }

        return defaultValue;
    }

    public virtual AuditLogInfo CreateAuditLogInfo()
    {
        var auditInfo = new AuditLogInfo
        {
            ApplicationName = Options.ApplicationName,
            TenantId = CurrentTenant.Id,
            TenantName = CurrentTenant.Name,
            UserId = CurrentUser.Id,
            UserName = CurrentUser.UserName,
            ClientId = CurrentClient.Id,
            CorrelationId = CorrelationIdProvider.Get(),
            ImpersonatorUserId = CurrentUser.FindImpersonatorUserId(),
            ImpersonatorTenantId = CurrentUser.FindImpersonatorTenantId(),
            ExecutionTime = Clock.Now
        };

        ExecutePreContributors(auditInfo);

        return auditInfo;
    }

    public virtual AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type type,
        MethodInfo method,
        object[] arguments)
    {
        return CreateAuditLogAction(auditLog, type, method, CreateArgumentsDictionary(method, arguments));
    }

    public virtual AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type type,
        MethodInfo method,
        IDictionary<string, object> arguments)
    {
        var actionInfo = new AuditLogActionInfo
        {
            ServiceName = type != null
                ? type.FullName
                : "",
            MethodName = method.Name,
            Parameters = SerializeConvertArguments(arguments),
            ExecutionTime = Clock.Now
        };

        //TODO Execute contributors

        return actionInfo;
    }

    protected virtual void ExecutePreContributors(AuditLogInfo auditLogInfo)
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

            foreach (var contributor in Options.Contributors)
            {
                try
                {
                    contributor.PreContribute(context);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                }
            }
        }
    }

    protected virtual string SerializeConvertArguments(IDictionary<string, object> arguments)
    {
        try
        {
            if (arguments.IsNullOrEmpty())
            {
                return "{}";
            }

            var dictionary = new Dictionary<string, object>();

            foreach (var argument in arguments)
            {
                if (argument.Value != null && Options.IgnoredTypes.Any(t => t.IsInstanceOfType(argument.Value)))
                {
                    dictionary[argument.Key] = null;
                }
                else
                {
                    dictionary[argument.Key] = argument.Value;
                }
            }

            return AuditSerializer.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            return "{}";
        }
    }

    protected virtual Dictionary<string, object> CreateArgumentsDictionary(MethodInfo method, object[] arguments)
    {
        var parameters = method.GetParameters();
        var dictionary = new Dictionary<string, object>();

        for (var i = 0; i < parameters.Length; i++)
        {
            dictionary[parameters[i].Name] = arguments[i];
        }

        return dictionary;
    }
}
