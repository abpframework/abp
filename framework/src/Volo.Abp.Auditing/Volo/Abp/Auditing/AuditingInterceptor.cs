using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.Auditing;

public class AuditingInterceptor : AbpInterceptor, ITransientDependency
{
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
                await ProceedByLoggingAsync(invocation, auditingHelper, auditingManager.Current);
            }
            else
            {
                var currentUser = serviceScope.ServiceProvider.GetRequiredService<ICurrentUser>();
                var unitOfWorkManager = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                await ProcessWithNewAuditingScopeAsync(invocation, auditingOptions, currentUser, auditingManager, auditingHelper, unitOfWorkManager);
            }
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

        if (!auditingHelper.ShouldSaveAudit(invocation.Method))
        {
            return false;
        }

        return true;
    }

    private static async Task ProceedByLoggingAsync(
        IAbpMethodInvocation invocation,
        IAuditingHelper auditingHelper,
        IAuditLogScope auditLogScope)
    {
        var auditLog = auditLogScope.Log;
        var auditLogAction = auditingHelper.CreateAuditLogAction(
            auditLog,
            invocation.TargetObject.GetType(),
            invocation.Method,
            invocation.Arguments
        );

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
            auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
            auditLog.Actions.Add(auditLogAction);
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
                await ProceedByLoggingAsync(invocation, auditingHelper, auditingManager.Current);

                Debug.Assert(auditingManager.Current != null);
                if (auditingManager.Current.Log.Exceptions.Any())
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
                if (await ShouldWriteAuditLogAsync(invocation, auditingManager.Current.Log, options, currentUser, hasError))
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
            invocation.Method.Name.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
