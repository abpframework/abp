using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.CmsKit.Admin.Contents;
using Xunit;

namespace Volo.CmsKit.Contents;

public class ContentAdminAppService_Tests : CmsKitApplicationTestBase
{
    private readonly IContentAdminAppService _contentAdminAppService;

    public ContentAdminAppService_Tests()
    {
        _contentAdminAppService = GetRequiredService<IContentAdminAppService>();
    }

    [Fact]
    public async Task ShouldGet_PagedListAsync()
    {
        var widgets = await _contentAdminAppService.GetWidgetsAsync();

        widgets.Items.Count.ShouldBe(0);
        widgets.Items.Any().ShouldBeFalse();
    }

}
