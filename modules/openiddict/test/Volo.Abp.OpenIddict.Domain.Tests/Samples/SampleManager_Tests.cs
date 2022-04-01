using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.OpenIddict.Samples;

public class SampleManager_Tests : OpenIddictDomainTestBase
{
    //private readonly SampleManager _sampleManager;

    public SampleManager_Tests()
    {
        //_sampleManager = GetRequiredService<SampleManager>();
    }

    [Fact]
    public async Task Method1Async()
    {
        var q = new CultureInfo("zh").Parent.Name;
    }
}
