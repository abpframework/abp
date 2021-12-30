using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobStore_Tests : BackgroundJobsDomainTestBase
{
    private readonly IBackgroundJobStore _backgroundJobStore;

    public BackgroundJobStore_Tests()
    {
        _backgroundJobStore = GetRequiredService<IBackgroundJobStore>();
    }

    [Fact]
    public async Task InsertAsync()
    {
        var jobInfo = new BackgroundJobInfo
        {
            Id = Guid.NewGuid(),
            JobArgs = "args1",
            JobName = "name1"
        };

        await _backgroundJobStore.InsertAsync(
            jobInfo
        );

        var jobInfo2 = await _backgroundJobStore.FindAsync(jobInfo.Id);
        jobInfo2.ShouldNotBeNull();
        jobInfo2.Id.ShouldBe(jobInfo.Id);
        jobInfo2.JobArgs.ShouldBe(jobInfo.JobArgs);
        jobInfo2.JobName.ShouldBe(jobInfo.JobName);
    }
}
