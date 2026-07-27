using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

public class FakeAPipelineContributor : FakeMarkerPipelineContributorBase, ITransientDependency
{
    public FakeAPipelineContributor()
        : base("A>")
    {
    }
}
