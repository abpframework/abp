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
        public async Task ShouldFindByUrlAsync()
        {
            var page = await _pageAppService.FindByUrlAsync(_data.Page_1_Url);

            page.ShouldNotBeNull();
            page.Title.ShouldBe(_data.Page_1_Title);
        }
        
        [Fact]
        public async Task ShouldNotGetByUrlAsync()
        {
            var page = await _pageAppService.FindByUrlAsync("not-exist-url");
            
            page.ShouldBeNull();
        }
    }
}