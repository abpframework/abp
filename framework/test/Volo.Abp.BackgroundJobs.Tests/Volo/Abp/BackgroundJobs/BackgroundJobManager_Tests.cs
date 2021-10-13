using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobManager_Tests : BackgroundJobsTestBase
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IBackgroundJobStore _backgroundJobStore;
        private readonly IClock _clock;

        public BackgroundJobManager_Tests()
        {
            _backgroundJobManager = GetRequiredService<IBackgroundJobManager>();
            _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
            _clock = GetRequiredService<IClock>();
        }

        [Fact]
        public async Task Should_Store_Jobs()
        {
            var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyJobArgs("42"));
            jobIdAsString.ShouldNotBe(default);
            (await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString))).ShouldNotBeNull();
        }
        
        [Fact]
        public async Task Should_Store_Jobs_With_Delay()
        {
            var now = _clock.Now;
            var delay = TimeSpan.FromMinutes(15);
            var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyJobArgs("42"), delay: delay);
            jobIdAsString.ShouldNotBe(default);
            var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
            jobInfo.ShouldNotBeNull();
            jobInfo.LastTryTime.ShouldBeNull();
            jobInfo.NextTryTime.ShouldBeGreaterThanOrEqualTo(now.Add(delay));
        }

        [Fact]
        public async Task Should_Store_Jobs_With_Execution_Time()
        {
            var executionTime = _clock.Now.Add(TimeSpan.FromMinutes(15));
            var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyJobArgs("42"), executionTime);
            jobIdAsString.ShouldNotBe(default);
            var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
            jobInfo.ShouldNotBeNull();
            jobInfo.LastTryTime.ShouldBeNull();
            jobInfo.NextTryTime.ShouldBe(executionTime);
        }

        [Fact]
        public async Task Should_Store_Async_Jobs()
        {
            var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyAsyncJobArgs("42"));
            jobIdAsString.ShouldNotBe(default);
            (await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString))).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Store_Async_Jobs_With_Delay()
        {
            var now = _clock.Now;
            var delay = TimeSpan.FromMinutes(15);
            var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyAsyncJobArgs("42"), delay: delay);
            jobIdAsString.ShouldNotBe(default);
            var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
            jobInfo.ShouldNotBeNull();
            jobInfo.LastTryTime.ShouldBeNull();
            jobInfo.NextTryTime.ShouldBeGreaterThanOrEqualTo(now.Add(delay));
        }

        [Fact]
        public async Task Should_Store_Async_Jobs_With_Execution_Time()
        {
            var executionTime = _clock.Now.Add(TimeSpan.FromMinutes(15));
            var jobIdAsString = await _backgroundJobManager.EnqueueAsync(new MyAsyncJobArgs("42"), executionTime);
            jobIdAsString.ShouldNotBe(default);
            var jobInfo = await _backgroundJobStore.FindAsync(Guid.Parse(jobIdAsString));
            jobInfo.ShouldNotBeNull();
            jobInfo.LastTryTime.ShouldBeNull();
            jobInfo.NextTryTime.ShouldBe(executionTime);
        }
    }
}
