using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;
using Volo.CmsKit.Admin.Contents;
using Xunit;

namespace Volo.CmsKit.Contents
{
    public class ContentAdminAppService_Tests : CmsKitApplicationTestBase
    {
        private ICurrentUser _currentUser;
        private readonly CmsKitTestData _data;
        private readonly IContentAdminAppService _service;
        public ContentAdminAppService_Tests()
        {
            _data = GetRequiredService<CmsKitTestData>();
            _service = GetRequiredService<IContentAdminAppService>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _currentUser = Substitute.For<ICurrentUser>();
            services.AddSingleton(_currentUser);
        }

        [Fact]
        public async Task ShouldGetListAsync()
        {
            var result = await _service.GetListAsync(new ContentGetListInput());

            result.ShouldNotBeNull();
            result.Items.ShouldNotBeEmpty();
            result.Items.Count.ShouldBe(4);
        }

        [Fact]
        public async Task ShouldGetAsync()
        {
            var result = await _service.GetAsync(_data.Content_1_Id);

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldCreateAsync()
        {
            var created = await _service.CreateAsync(new ContentCreateDto
            {
                EntityId = "test_case_id",
                EntityType = "test_case_type",
                Value = "Some long content"
            });

            created.ShouldNotBeNull();
        }

        [Fact]
        public async Task ShouldNotCreateWithSameParametersAsync()
        {
            var entityTtype = "My.Awesome.Book";
            var entityId = "1";

            await _service.CreateAsync(new ContentCreateDto
            {
                EntityId = entityId,
                EntityType = entityTtype,
                Value = "Some long content"
            });

            await Should.ThrowAsync<Exception>(async () =>
                await _service.CreateAsync(new ContentCreateDto
                {
                    EntityId = entityId,
                    EntityType = entityTtype,
                    Value = "Yet another long content"
                })
            );
        }

        [Fact]
        public async Task ShouldUpdateAsync()
        {
            string newValue = "Newly created fresh value";
            var updateDto = new ContentUpdateDto
            {
                Value = newValue
            };

            await _service.UpdateAsync(_data.Content_1_Id, updateDto);

            var content = await _service.GetAsync(_data.Content_1_Id);
            content.ShouldNotBeNull();
            content.Value.ShouldBe(newValue);
        }

        [Fact]
        public async Task ShouldDeleteAsync()
        {
            await _service.DeleteAsync(_data.Content_2_Id);

            await Should.ThrowAsync<EntityNotFoundException>(async () => await _service.GetAsync(_data.Content_2_Id));
        }

        [Fact]
        public async Task ShouldThrowEntityNotFoundExceptionWhileDeletingNonExistingAsync()
        {
            await Should.ThrowAsync<EntityNotFoundException>(async () => 
                await _service.DeleteAsync(Guid.NewGuid())); // Freshly generated non-existing id.
        }

        [Fact]
        public async Task ShouldThrowEntityNotFoundExceptionWhileDeletingAlreadyDeletedAsync()
        {
            await _service.DeleteAsync(_data.Content_2_Id);

            Should.ThrowAsync<EntityNotFoundException>(async () =>
                await _service.DeleteAsync(_data.Content_2_Id));
        }
    }
}
