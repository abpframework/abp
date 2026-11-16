using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.Auditing;

public class AuditingInterceptor : AbpInterceptor, ITransientDependency
{
    public const string AsyncEnumerationPropertyName = "AsyncEnumeration";

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuditingInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        using (var serviceScope = _serviceScopeFactory.CreateScope())
        {
            var auditingHelper = serviceScope.ServiceProvider.GetRequiredService<IAuditingHelper>();
            var auditingOptions = serviceScope.ServiceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;

            if (!ShouldIntercept(invocation, auditingOptions, auditingHelper))
            {
                await invocation.ProceedAsync();
                return;
            }

            var auditingManager = serviceScope.ServiceProvider.GetRequiredService<IAuditingManager>();
            if (auditingManager.Current != null)
            {
                await ProceedByLoggingAsync(invocation, auditingOptions, auditingHelper, auditingManager.Current);
            }
            else
            {
                var currentUser = serviceScope.ServiceProvider.GetRequiredService<ICurrentUser>();
                var unitOfWorkManager = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                await ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, currentUser, auditingManager, auditingHelper, unitOfWorkManager);
            }
        }

        if (TypeHelper.IsAsyncEnumerable(invocation.Method.ReturnType))
        {
            WrapAsyncEnumerable(invocation);
        }
    }

    protected virtual bool ShouldIntercept(IAbpMethodInvocation invocation,
        AbpAuditingOptions options,
        IAuditingHelper auditingHelper)
    {
        if (!options.IsEnabled)
        {
            return false;
        }

        if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Auditing))
        {
            return false;
        }

        if (!auditingHelper.ShouldSaveAudit(
                invocation.Method,
                ignoreIntegrationServiceAttribute: options.IsEnabledForIntegrationServices))
        {
            return false;
        }

        return true;
    }

    private static async Task ProceedByLoggingAsync(
        IAbpMethodInvocation invocation,
        AbpAuditingOptions options,
        IAuditingHelper auditingHelper,
        IAuditLogScope auditLogScope)
    {
        var auditLog = auditLogScope.Log;

        AuditLogActionInfo? auditLogAction = null;
        if (!options.DisableLogActionInfo)
        {
            auditLogAction = auditingHelper.CreateAuditLogAction(
                auditLog,
                invocation.TargetObject?.GetType(),
                invocation.Method,
                invocation.Arguments
            );
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await invocation.ProceedAsync();
        }
        catch (Exception ex)
        {
            auditLog.Exceptions.Add(ex);
            throw;
        }
        finally
        {
            stopwatch.Stop();

            if (auditLogAction != null)
            {
                auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditLog.Actions.Add(auditLogAction);
            }
        }
    }

    private async Task ProcessWithNewAuditingScopeAsync(
        IAbpMethodInvocation invocation,
        AbpAuditingOptions options,
        ICurrentUser currentUser,
        IAuditingManager auditingManager,
        IAuditingHelper auditingHelper,
        IUnitOfWorkManager unitOfWorkManager)
    {
        var hasError = false;
        using (var saveHandle = auditingManager.BeginScope())
        {
            try
            {
                await ProceedByLoggingAsync(invocation, options, auditingHelper, auditingManager.Current!);

                Debug.Assert(auditingManager.Current != null);
                if (auditingManager.Current!.Log.Exceptions.Any())
                {
                    hasError = true;
                }
            }
            catch (Exception)
            {
                hasError = true;
                throw;
            }
            finally
            {
                if (await ShouldWriteAuditLogAsync(invocation, auditingManager.Current!.Log, options, currentUser, hasError))
                {
                    if (unitOfWorkManager.Current != null)
                    {
                        try
                        {
                            await unitOfWorkManager.Current.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            if (!auditingManager.Current.Log.Exceptions.Contains(ex))
                            {
                                auditingManager.Current.Log.Exceptions.Add(ex);
                            }
                        }
                    }

                    await saveHandle.SaveAsync();
                }
            }
        }
    }

    private void WrapAsyncEnumerable(IAbpMethodInvocation invocation)
    {
        invocation.WrapAsyncEnumerable(() => BeginEnumerationScope(invocation));
    }

    private IAbpInterceptionScope BeginEnumerationScope(IAbpMethodInvocation invocation)
    {
        var serviceScope = _serviceScopeFactory.CreateScope();
        IAuditLogSaveHandle? saveHandle = null;
        try
        {
            var serviceProvider = serviceScope.ServiceProvider;
            var auditingHelper = serviceProvider.GetRequiredService<IAuditingHelper>();
            if (!auditingHelper.IsAuditingEnabled())
            {
                return new AbpInterceptionScope(serviceScope);
            }

            var options = serviceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
            var auditingManager = serviceProvider.GetRequiredService<IAuditingManager>();

            if (auditingManager.Current == null)
            {
                saveHandle = auditingManager.BeginScope();
            }

            var auditLog = auditingManager.Current!.Log;

            AuditLogActionInfo? auditLogAction = null;
            if (!options.DisableLogActionInfo)
            {
                auditLogAction = auditingHelper.CreateAuditLogAction(
                    auditLog,
                    invocation.TargetObject?.GetType(),
                    invocation.Method,
                    invocation.Arguments
                );
                auditLogAction.ExtraProperties[AsyncEnumerationPropertyName] = true;
            }

            return new AuditLogEnumerationScope(
                serviceScope,
                auditLog,
                auditLogAction,
                saveHandle,
                hasError => ShouldWriteAuditLogAsync(invocation, auditLog, options, serviceProvider.GetRequiredService<ICurrentUser>(), hasError));
        }
        catch
        {
            try
            {
                saveHandle?.Dispose();
            }
            finally
            {
                serviceScope.Dispose();
            }

            throw;
        }
    }

    private async Task<bool> ShouldWriteAuditLogAsync(
        IAbpMethodInvocation invocation,
        AuditLogInfo auditLogInfo,
        AbpAuditingOptions options,
        ICurrentUser currentUser,
        bool hasError)
    {
        foreach (var selector in options.AlwaysLogSelectors)
        {
            if (await selector(auditLogInfo))
            {
                return true;
            }
        }

        if (options.AlwaysLogOnException && hasError)
        {
            return true;
        }

        if (!options.IsEnabledForAnonymousUsers && !currentUser.IsAuthenticated)
        {
            return false;
        }

        if (!options.IsEnabledForGetRequests &&
            (string.Equals(auditLogInfo.HttpMethod, "Get", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(auditLogInfo.HttpMethod, "Head", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(auditLogInfo.HttpMethod, "Query", StringComparison.OrdinalIgnoreCase) ||
             invocation.Method.Name.StartsWith("Get", StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        return true;
    }
}
