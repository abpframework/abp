using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.CmsKit.PublicWebTests;

public class PublicLookupCache_Http_Tests : CmsKitPublicWebTestBase
{
    [Theory]
    [InlineData(12)]
    [InlineData(257)]
    public async Task Unknown_Dynamic_Page_Routes_Should_Allow_The_Normal_Route_To_Continue(int length)
    {
        var recorder = GetRequiredService<LookupRecordingCache>();
        recorder.Writes.Clear();
        using var response = await Client.GetAsync("/cms-fallback/" + new string('a', length));
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("Fallback");
        recorder.Writes.ShouldNotContain(key => key.Contains("CmsPage_"));
    }
}
