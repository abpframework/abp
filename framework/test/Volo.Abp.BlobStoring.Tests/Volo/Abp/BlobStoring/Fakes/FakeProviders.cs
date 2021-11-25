using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

public class FakeProviders : ISingletonDependency
{
    public FakeBlobProvider1 Provider1 { get; }

    public FakeBlobProvider2 Provider2 { get; }

    public FakeProviders(IEnumerable<IBlobProvider> providers)
    {
        Provider1 = providers.OfType<FakeBlobProvider1>().Single();
        Provider2 = providers.OfType<FakeBlobProvider2>().Single();
    }
}
