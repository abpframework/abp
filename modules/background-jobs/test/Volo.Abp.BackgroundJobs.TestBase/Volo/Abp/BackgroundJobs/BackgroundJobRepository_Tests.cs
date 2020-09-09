using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.BackgroundJobs
{
    public abstract class BackgroundJobRepository_Tests<TStartupModule> : BackgroundJobsTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IBackgroundJobRepository _backgroundJobRepository;
        private readonly IClock _clock;

        protected BackgroundJobRepository_Tests()
        {
            _backgroundJobRepository = GetRequiredService<IBackgroundJobRepository>();
            _clock = GetRequiredService<IClock>();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public async Task GetWaitingListAsync(int maxResultCount)
        {
            var backgroundJobs = await _backgroundJobRepository.GetWaitingListAsync(maxResultCount);

            backgroundJobs.Count.ShouldBeGreaterThan(0);
            backgroundJobs.Count.ShouldBeLessThanOrEqualTo(maxResultCount);

            backgroundJobs.ForEach(j => j.IsAbandoned.ShouldBeFalse());
            backgroundJobs.ForEach(j => j.NextTryTime.ShouldBeLessThanOrEqualTo(_clock.Now.AddSeconds(1))); //1 second tolerance
        }
    }
}
