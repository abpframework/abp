using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Uow;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Contents;
using Xunit;

namespace Volo.CmsKit.Pages
{
    public class PageAdminAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly CmsKitTestData _data;
        private readonly IPageAdminAppService _pageAdminAppService;

        private readonly IPageRepository _pageRepository;
        private readonly IContentRepository _contentRepository;

        public PageAdminAppService_Tests()
        {
            _data = GetRequiredService<CmsKitTestData>();
            _pageAdminAppService = GetRequiredService<IPageAdminAppService>();
            _pageRepository = GetRequiredService<IPageRepository>();
            _contentRepository = GetRequiredService<IContentRepository>();
        }

        [Fact]
        public async Task ShouldGetAsync()
        {
            var page = await _pageAdminAppService.GetAsync(_data.Page_1_Id);
            
            page.Description.ShouldBe(_data.Page_1_Description);
        }
        
        [Fact]
        public async Task ShouldCreateAsync()
        {
            var dto = new CreatePageInputDto
            {
                Title = "test",
                Url = "test-url"
            };

            await Should.NotThrowAsync(async () => await _pageAdminAppService.CreatePageAsync(dto));

            var page = await _pageRepository.GetByUrlAsync(dto.Url);
            
            page.Title.ShouldBe(dto.Title);
        }
        
        [Fact]
        public async Task ShouldNotCreateAsync()
        {
            var dto = new CreatePageInputDto
            {
                Title = "test",
                Url = _data.Page_1_Url
            };

            await Should.ThrowAsync<Exception>(async () => await _pageAdminAppService.CreatePageAsync(dto));
        }
        
        [Fact]
        public async Task ShouldCreateWithContentAsync()
        {
            var dto = new CreatePageWithContentInputDto
            {
                Title = "test",
                Url = "test-url",
                Content = "my-test-content"
            };

            await Should.NotThrowAsync(async () => await _pageAdminAppService.CreatePageWithContentAsync(dto));

            var page = await _pageRepository.GetByUrlAsync(dto.Url);

            var content = await _contentRepository.GetAsync(nameof(Page), page.Id.ToString());
            
            content.Value.ShouldBe(dto.Content);
        }
        
        [Fact]
        public async Task ShouldNotCreateWithContentAsync()
        {
            var dto = new CreatePageWithContentInputDto
            {
                Title = "test",
                Url = _data.Page_1_Url,
                Content = "my-test-content"
            };

            await Should.ThrowAsync<Exception>(async () => await _pageAdminAppService.CreatePageWithContentAsync(dto));
        }

        [Fact]
        public async Task ShouldUpdatePageAsync()
        {
            var dto = new UpdatePageInputDto
            {
                Title = _data.Page_1_Title + "++",
                Description = "new description",
                Url = _data.Page_1_Url+ "test"
            };

            await Should.NotThrowAsync(async () => await _pageAdminAppService.UpdatePageAsync(_data.Page_1_Id, dto));

            var updatedPage = await _pageRepository.GetAsync(_data.Page_1_Id);
            
            updatedPage.Title.ShouldNotBe(_data.Page_1_Title);
            updatedPage.Title.ShouldBe(dto.Title);
            
            updatedPage.Url.ShouldNotBe(_data.Page_1_Url);
            updatedPage.Url.ShouldBe(dto.Url);
            
            updatedPage.Description.ShouldNotBe(_data.Page_1_Description);
            updatedPage.Description.ShouldBe(dto.Description);
        }
        
        [Fact]
        public async Task ShouldNotUpdatePageAsync()
        {
            var dto = new UpdatePageInputDto
            {
                Title = _data.Page_1_Title + "++",
                Description = "new description",
                Url = _data.Page_2_Url
            };

            await Should.ThrowAsync<Exception>(async () => await _pageAdminAppService.UpdatePageAsync(_data.Page_1_Id, dto));
        }

        [Fact]
        public async Task ShouldBeExistAsync()
        {
            var dto = new CheckUrlInputDto
            {
                Url = _data.Page_1_Url
            };

            var doesExist = await _pageAdminAppService.DoesUrlExistAsync(dto);
            
            doesExist.ShouldBeTrue();
        }
        
        [Fact]
        public async Task ShouldNotBeExistAsync()
        {
            var dto = new CheckUrlInputDto
            {
                Url = _data.Page_1_Url+ "+"
            };

            var doesExist = await _pageAdminAppService.DoesUrlExistAsync(dto);
            
            doesExist.ShouldBeFalse();
        }
        
        [Fact]
        public async Task ShouldUpdateContentAsync()
        {
            var dto = new UpdatePageContentInputDto
            {
                Content = "my-new-content"
            };

            await Should.NotThrowAsync(async () => await _pageAdminAppService.UpdatePageContentAsync(_data.Page_1_Id, dto));

            var content = await _contentRepository.GetAsync(nameof(Page), _data.Page_1_Id.ToString());
            
            content.Value.ShouldBe(dto.Content);
        }
        
        [Fact]
        public async Task ShouldDeleteAsync()
        {
            await _pageAdminAppService.DeleteAsync(_data.Page_1_Id);

            await Should.ThrowAsync<Exception>(async () => await _pageRepository.GetAsync(_data.Page_1_Id));
            
            await Should.ThrowAsync<Exception>(async () => await _contentRepository.GetAsync(nameof(Page), _data.Page_1_Id.ToString()));
        }
    }
}