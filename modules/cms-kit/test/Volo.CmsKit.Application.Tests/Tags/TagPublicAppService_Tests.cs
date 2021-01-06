using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Clients;
using Volo.Abp.Users;
using Volo.Abp.Validation;
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
            var list = await _tagAppService.GetAllRelatedTagsAsync(new GetRelatedTagsInput
            {
                EntityType = _cmsKitTestData.Content_1_EntityType,
                EntityId = _cmsKitTestData.EntityId1
            });

            list.ShouldNotBeEmpty();
            list.Count.ShouldBe(2);
        }

        [Fact]
        public async Task ShouldntGet_GetAllRelatedTagsAsync()
        {
            var list = await _tagAppService.GetAllRelatedTagsAsync(new GetRelatedTagsInput
            {
                EntityType = "any_other_type",
                EntityId = "1"
            });

            list.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetRelatedTagsAsync_ShouldThrowValidationException_WithoutEntityType()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () => 
                await _tagAppService.GetAllRelatedTagsAsync(new GetRelatedTagsInput
                {
                    EntityType = null,
                    EntityId = _cmsKitTestData.EntityId1
                }));
        }

        [Fact]
        public async Task GetRelatedTagsAsync_ShouldThrowValidationException_WithoutEntityId()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () => 
                await _tagAppService.GetAllRelatedTagsAsync(new GetRelatedTagsInput
                {
                    EntityType = null,
                    EntityId = _cmsKitTestData.EntityId1
                }));
        }
    }
}
