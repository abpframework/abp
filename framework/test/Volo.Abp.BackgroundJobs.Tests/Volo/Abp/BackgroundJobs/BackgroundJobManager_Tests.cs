using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobManager_Tests : BackgroundJobsTestBase
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IBackgroundJobStore _backgroundJobStore;

        public BackgroundJobManager_Tests()
        {
            _backgroundJobManager = GetRequiredService<IBackgroundJobManager>();
            _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
        }

        [Fact]
        public async Task Should_Store_Jobs()
        {
            var jobId = await _backgroundJobManager.EnqueueAsync(new MyJobArgs("42"));
            jobId.ShouldNotBe(default);
            (await _backgroundJobStore.FindAsync(jobId)).ShouldNotBeNull();
        }

        [BackgroundJobName("TestJobs.MyJob")]
        private class MyJobArgs
        {
            public string Value { get; set; }

            public MyJobArgs()
            {
                
            }

            public MyJobArgs(string value)
            {
                Value = value;
            }
        }
    }
}
