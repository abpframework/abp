using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Volo.Abp.Quartz
{
    /// <summary>
    /// Get the job from the dependency injection
    /// </summary>
    public class AbpQuartzJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new ConcurrentDictionary<IJob, IServiceScope>();

        public AbpQuartzJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _serviceProvider.CreateScope();
            var job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            if (job == null)
            {
                throw new ArgumentException("Given job does not implement IJob");
            }
            _scopes.TryAdd(job, scope);
            return job;
        }

        public void ReturnJob(IJob job)
        {
            _scopes.TryRemove(job, out var serviceScope);
            serviceScope?.Dispose();
        }
    }
}
