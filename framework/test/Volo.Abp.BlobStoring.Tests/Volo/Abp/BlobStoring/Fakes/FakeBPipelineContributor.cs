using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

public class FakeBPipelineContributor : FakeMarkerPipelineContributorBase, ITransientDependency
{
    public FakeBPipelineContributor()
        : base("B>")
    {
    }
}
