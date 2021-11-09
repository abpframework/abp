using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.SignalR.Auditing
{
    public class AbpAuditHubFilter : IHubFilter
    {
        public virtual async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            var options = invocationContext.ServiceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
            if (!options.IsEnabled)
            {
                return await next(invocationContext);
            }

            var hasError = false;
            var auditingManager = invocationContext.ServiceProvider.GetRequiredService<IAuditingManager>();
            using (var saveHandle = auditingManager.BeginScope())
            {
                Debug.Assert(auditingManager.Current != null);
                object result;
                try
                {
                    result = await next(invocationContext);

                    if (auditingManager.Current.Log.Exceptions.Any())
                    {
                        hasError = true;
                    }
                }
                catch (Exception ex)
                {
                    hasError = true;

                    if (!auditingManager.Current.Log.Exceptions.Contains(ex))
                    {
                        auditingManager.Current.Log.Exceptions.Add(ex);
                    }

                    throw;
                }
                finally
                {
                    if (ShouldWriteAuditLog(invocationContext.ServiceProvider, hasError))
                    {
                        var unitOfWorkManager = invocationContext.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                        if (unitOfWorkManager.Current != null)
                        {
                            await unitOfWorkManager.Current.SaveChangesAsync();
                        }

                        await saveHandle.SaveAsync();
                    }
                }

                return result;
            }
        }

        private bool ShouldWriteAuditLog(IServiceProvider serviceProvider, bool hasError)
        {
            var options = serviceProvider.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
            if (options.AlwaysLogOnException && hasError)
            {
                return true;
            }

            if (!options.IsEnabledForAnonymousUsers && !serviceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
            {
                return false;
            }

            var auditingManager = serviceProvider.GetRequiredService<IAuditingManager>();
            if (auditingManager.Current != null && auditingManager.Current.Log.Actions.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
