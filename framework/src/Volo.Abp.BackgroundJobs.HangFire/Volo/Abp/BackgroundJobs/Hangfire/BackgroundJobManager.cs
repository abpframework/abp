using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.HangFire.Volo.Abp.BackgroundJobs.Hangfire
{
    public class BackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        public Task<Guid> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            throw new NotImplementedException();
        }
    }
}
