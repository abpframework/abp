using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobsTestData : ISingletonDependency
{
    public Guid JobId1 { get; } = Guid.NewGuid();
    public Guid JobId2 { get; } = Guid.NewGuid();
    public Guid JobId3 { get; } = Guid.NewGuid();
}
