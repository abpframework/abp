using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Pages
{
    public abstract class PageRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly IPageRepository _pageRepository;

        protected PageRepository_Test()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _pageRepository = GetRequiredService<IPageRepository>();
        }

        [Fact]
        public async Task ShouldGetByUrlAsync()
        {
            var page = await _pageRepository.GetByUrlAsync(_cmsKitTestData.Page_1_Url);

            page.ShouldNotBeNull();
            page.Description.ShouldBe(_cmsKitTestData.Page_1_Description);
        }
        
        [Fact]
        public async Task ShouldFindByUrlAsync()
        {
            var page = await _pageRepository.FindByUrlAsync(_cmsKitTestData.Page_1_Url);

            page.ShouldNotBeNull();
            page.Description.ShouldBe(_cmsKitTestData.Page_1_Description);
        }
        
        [Fact]
        public async Task ShouldNotFindByUrlAsync()
        {
            var page = await _pageRepository.FindByUrlAsync("not-exist-lyrics");

            page.ShouldBeNull();
        }
        
        [Fact]
        public async Task ShouldBeExistAsync()
        {
            var page = await _pageRepository.DoesExistAsync(_cmsKitTestData.Page_1_Url);

            page.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ShouldNotBeExistAsync()
        {
            var page = await _pageRepository.DoesExistAsync("not-exist-lyrics");

            page.ShouldBeFalse();
        }
    }
}