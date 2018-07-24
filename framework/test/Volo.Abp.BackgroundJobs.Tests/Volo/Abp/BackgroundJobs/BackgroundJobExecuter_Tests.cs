using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Json;
using Xunit;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobExecuter_Tests : BackgroundJobsTestBase
    {
        private readonly IBackgroundJobExecuter _backgroundJobExecuter;
        private readonly IJsonSerializer _jsonSerializer;

        public BackgroundJobExecuter_Tests()
        {
            _backgroundJobExecuter = GetRequiredService<IBackgroundJobExecuter>();
            _jsonSerializer = GetRequiredService<IJsonSerializer>();
        }

        [Fact]
        public async Task Should_Execute_Tasks()
        {
            //Arrange

            var jobObject = GetRequiredService<MyJob>();
            jobObject.ExecutedValues.ShouldBeEmpty();

            //Act

            _backgroundJobExecuter.Execute(
                new JobExecutionContext(
                    BackgroundJobNameAttribute.GetName<MyJobArgs>(),
                    _jsonSerializer.Serialize(new MyJobArgs("42"))
                )
            );

            //Assert

            jobObject.ExecutedValues.ShouldContain("42");
        }
    }
}