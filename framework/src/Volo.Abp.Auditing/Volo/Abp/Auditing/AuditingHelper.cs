using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Volo.Abp.Auditing
{
    public class AuditingHelper : IAuditingHelper, ITransientDependency
    {
        protected ILogger<AuditingHelper> Logger { get; }
        protected IAuditingStore AuditingStore { get; }
        protected ICurrentUser CurrentUser { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IClock Clock { get; }
        protected AuditingOptions Options;
        protected IAuditSerializer AuditSerializer;
        protected IServiceProvider ServiceProvider;

        public AuditingHelper(
            IAuditSerializer auditSerializer,
            IOptions<AuditingOptions> options,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IAuditingStore auditingStore,
            ILogger<AuditingHelper> logger,
            IServiceProvider serviceProvider)
        {
            Options = options.Value;
            AuditSerializer = auditSerializer;
            CurrentUser = currentUser;
            CurrentTenant = currentTenant;
            Clock = clock;
            AuditingStore = auditingStore;

            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        public virtual bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false)
        {
            if (!Options.IsEnabled)
            {
                return false;
            }

            if (!Options.IsEnabledForAnonymousUsers && !CurrentUser.IsAuthenticated)
            {
                return false;
            }

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
                if (AuditingInterceptorRegistrar.ShouldAuditTypeByDefault(classType))
                {
                    return true;
                }
            }

            return defaultValue;
        }

        public virtual AuditLogInfo CreateAuditLogInfo()
        {
            var auditInfo = new AuditLogInfo
            {
                TenantId = CurrentTenant.Id,
                UserId = CurrentUser.Id,
                //ImpersonatorUserId = AbpSession.ImpersonatorUserId, //TODO: Impersonation system is not available yet!
                //ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
                ExecutionTime = Clock.Now
            };

            ExecuteContributors(auditInfo);

            return auditInfo;
        }

        public virtual AuditLogActionInfo CreateAuditLogAction(Type type, MethodInfo method, object[] arguments)
        {
            return CreateAuditLogAction(type, method, CreateArgumentsDictionary(method, arguments));
        }

        public virtual AuditLogActionInfo CreateAuditLogAction(Type type, MethodInfo method, IDictionary<string, object> arguments)
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

        protected virtual void ExecuteContributors(AuditLogInfo auditLogInfo)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

                foreach (var contributor in Options.Contributors)
                {
                    try
                    {
                        contributor.ContributeAsync(context);
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
}