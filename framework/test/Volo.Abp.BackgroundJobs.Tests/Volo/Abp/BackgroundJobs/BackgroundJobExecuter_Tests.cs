using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobExecuter_Tests : BackgroundJobsTestBase
    {
        private readonly IBackgroundJobExecuter _backgroundJobExecuter;

        public BackgroundJobExecuter_Tests()
        {
            _backgroundJobExecuter = GetRequiredService<IBackgroundJobExecuter>();
        }

        [Fact]
        public async Task Should_Execute_Tasks()
        {
            //Arrange

            var jobObject = GetRequiredService<MyJob>();
            jobObject.ExecutedValues.ShouldBeEmpty();

            //Act

            await _backgroundJobExecuter.ExecuteAsync(
                new JobExecutionContext(
                    ServiceProvider,
                    typeof(MyJob),
                    new MyJobArgs("42")
                )
            );

            //Assert

            jobObject.ExecutedValues.ShouldContain("42");
        }

        [Fact]
        public async Task Should_Execute_Async_Tasks()
        {
            //Arrange

            var jobObject = GetRequiredService<MyAsyncJob>();
            jobObject.ExecutedValues.ShouldBeEmpty();

            //Act

            await _backgroundJobExecuter.ExecuteAsync(
                new JobExecutionContext(
                    ServiceProvider,
                    typeof(MyAsyncJob),
                    new MyAsyncJobArgs("42")
                )
            );

            //Assert

            jobObject.ExecutedValues.ShouldContain("42");
        }
    }
}