using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.CmsKit.Public.Pages;
using Xunit;

namespace Volo.CmsKit.Pages
{
    public class PagePublicAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly CmsKitTestData _data;
        private readonly IPageAppService _pageAppService;
        
        public PagePublicAppService_Tests()
        {
            _data = GetRequiredService<CmsKitTestData>();
            _pageAppService = GetRequiredService<IPageAppService>();
        }

        [Fact]
        public async Task ShouldGetByUrlAsync()
        {
            await Should.NotThrowAsync(async () => await _pageAppService.GetByUrlAsync(_data.Page_1_Url));
        }
        
        [Fact]
        public async Task ShouldNotGetByUrlAsync()
        {
            await Should.ThrowAsync<Exception>(async () => await _pageAppService.GetByUrlAsync("not-exist-url"));
        }
    }
}