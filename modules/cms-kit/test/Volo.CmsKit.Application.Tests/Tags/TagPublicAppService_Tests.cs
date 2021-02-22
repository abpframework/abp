using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Users;
using Volo.CmsKit.Public.Tags;
using Xunit;

namespace Volo.CmsKit.Tags
{
    public class TagPublicAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly ITagAppService _tagAppService;
        private ICurrentUser _currentUser;
        private readonly CmsKitTestData _cmsKitTestData;

        public TagPublicAppService_Tests()
        {
            _tagAppService = GetRequiredService<ITagAppService>();
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _currentUser = Substitute.For<ICurrentUser>();
            services.AddSingleton(_currentUser);
        }

        [Fact]
        public async Task GetAllRelatedTagsAsync()
        {
            var list = await _tagAppService.GetAllRelatedTagsAsync(_cmsKitTestData.Content_1_EntityType,
                _cmsKitTestData.EntityId1);

            list.ShouldNotBeEmpty();
            list.Count.ShouldBe(2);
        }

        [Fact]
        public async Task ShouldntGet_GetAllRelatedTagsAsync()
        {
            var list = await _tagAppService.GetAllRelatedTagsAsync("any_other_type", "1");

            list.ShouldBeEmpty();
        }
    }
}