using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobExecuter_Tests : BackgroundJobsTestBase
    {
        private readonly IBackgroundJobExecuter _backgroundJobExecuter;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IBackgroundJobStore _backgroundJobStore;

        public BackgroundJobExecuter_Tests()
        {
            _backgroundJobExecuter = GetRequiredService<IBackgroundJobExecuter>();
            _backgroundJobManager = GetRequiredService<IBackgroundJobManager>();
            _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
        }

        [Fact]
        public async Task Should_Execute_Tasks()
        {
            //Arrange

            var jobObject = GetRequiredService<MyJob>();
            jobObject.ExecutedValues.ShouldBeEmpty();

            var jobId = await _backgroundJobManager.EnqueueAsync(new MyJobArgs("42"));

            var job = await _backgroundJobStore.FindAsync(jobId);
            job.ShouldNotBeNull();

            //Act

            _backgroundJobExecuter.Execute(job);

            //Assert

            jobObject.ExecutedValues.ShouldContain("42");

            job = await _backgroundJobStore.FindAsync(jobId);
            job.ShouldBeNull(); //Because it's deleted after the execution
        }
    }
}