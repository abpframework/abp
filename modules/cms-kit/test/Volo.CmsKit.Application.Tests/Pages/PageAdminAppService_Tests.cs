using System;
using System.Linq;
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

        public PageAdminAppService_Tests()
        {
            _data = GetRequiredService<CmsKitTestData>();
            _pageAdminAppService = GetRequiredService<IPageAdminAppService>();
            _pageRepository = GetRequiredService<IPageRepository>();
        }

        [Fact]
        public async Task ShouldGetListAsync()
        {
            var input = new GetPagesInputDto();

            var pages = await _pageAdminAppService.GetListAsync(input);
            
            pages.TotalCount.ShouldBe(2);
            pages.Items.Count.ShouldBe(2);
            
            input.MaxResultCount = 1;
            
            var pages2 = await _pageAdminAppService.GetListAsync(input);
            
            pages2.TotalCount.ShouldBe(2);
            pages2.Items.Count.ShouldBe(1);
            pages2.Items.First().Title.ShouldBe(_data.Page_1_Title);
            
            input.SkipCount = 1;
            
            var pages3 = await _pageAdminAppService.GetListAsync(input);
            
            pages3.TotalCount.ShouldBe(2);
            pages3.Items.Count.ShouldBe(1);
            pages3.Items.First().Title.ShouldBe(_data.Page_2_Title);
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

            await Should.NotThrowAsync(async () => await _pageAdminAppService.CreateAsync(dto));

            var page = await _pageRepository.GetByUrlAsync(dto.Url);
            
            page.Title.ShouldBe(dto.Title);
        }
        
        [Fact]
        public async Task ShouldNotCreateExistUrlAsync()
        {
            var dto = new CreatePageInputDto
            {
                Title = "test",
                Url = _data.Page_1_Url
            };

            var exception = await Should.ThrowAsync<PageUrlAlreadyExistException>(async () => await _pageAdminAppService.CreateAsync(dto));
            
            exception.Code.ShouldBe(CmsKitErrorCodes.Pages.UrlAlreadyExist);
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

            await Should.NotThrowAsync(async () => await _pageAdminAppService.UpdateAsync(_data.Page_1_Id, dto));

            var updatedPage = await _pageRepository.GetAsync(_data.Page_1_Id);
            
            updatedPage.Title.ShouldNotBe(_data.Page_1_Title);
            updatedPage.Title.ShouldBe(dto.Title);
            
            updatedPage.Url.ShouldNotBe(_data.Page_1_Url);
            updatedPage.Url.ShouldBe(dto.Url);
            
            updatedPage.Description.ShouldNotBe(_data.Page_1_Description);
            updatedPage.Description.ShouldBe(dto.Description);
        }
        
        [Fact]
        public async Task ShouldNotUpdateWithExistUrlAsync()
        {
            var dto = new UpdatePageInputDto
            {
                Title = _data.Page_1_Title + "++",
                Description = "new description",
                Url = _data.Page_2_Url
            };

            await Should.ThrowAsync<Exception>(async () => await _pageAdminAppService.UpdateAsync(_data.Page_1_Id, dto));
        }

        [Fact]
        public async Task ShouldDeleteAsync()
        {
            await _pageAdminAppService.DeleteAsync(_data.Page_1_Id);

            await Should.ThrowAsync<Exception>(async () => await _pageRepository.GetAsync(_data.Page_1_Id));
        }
    }
}