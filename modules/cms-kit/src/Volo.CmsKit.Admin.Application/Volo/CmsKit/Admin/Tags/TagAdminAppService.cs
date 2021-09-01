using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
    [Authorize(CmsKitAdminPermissions.Tags.Default)]
    [RequiresGlobalFeature(typeof(TagsFeature))]
    public class TagAdminAppService : CmsKitAppServiceBase, ITagAdminAppService
    {
        protected ITagRepository Repository { get; }
        protected TagManager TagManager { get; }
        protected ITagDefinitionStore TagDefinitionStore { get; }
        protected IStringLocalizerFactory StringLocalizerFactory { get; }

        public TagAdminAppService(
            ITagRepository repository,
            TagManager tagManager,
            ITagDefinitionStore tagDefinitionStore,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            Repository = repository;
            TagManager = tagManager;
            TagDefinitionStore = tagDefinitionStore;
            StringLocalizerFactory = stringLocalizerFactory;
        }

        [Authorize(CmsKitAdminPermissions.Tags.Create)]
        public async Task<TagDto> CreateAsync(TagCreateDto input)
        {
            var tag = await TagManager.CreateAsync(
                GuidGenerator.Create(),
                input.EntityType,
                input.Name);

            await Repository.InsertAsync(tag);

            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        [Authorize(CmsKitAdminPermissions.Tags.Update)]
        public async Task<TagDto> UpdateAsync(Guid id, TagUpdateDto input)
        {
            var tag = await TagManager.UpdateAsync(
                id,
                input.Name);

            tag.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

            await Repository.UpdateAsync(tag);

            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        [Authorize(CmsKitAdminPermissions.Tags.Default)]
        public virtual async Task<List<TagDefinitionDto>> GetTagDefinitionsAsync()
        {
            var definitions = await TagDefinitionStore.GetTagEntityTypeDefinitionListAsync();

            return definitions
                        .Select(s =>
                            new TagDefinitionDto
                            {
                                EntityType = s.EntityType,
                                DisplayName = s.DisplayName?.Localize(StringLocalizerFactory) ?? s.EntityType
                            })
                        .ToList();
        }

        [Authorize(CmsKitAdminPermissions.Tags.Default)]
        public async Task<TagDto> GetAsync(Guid id)
        {
            var tag = await Repository.GetAsync(id);

            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        [Authorize(CmsKitAdminPermissions.Tags.Default)]
        public async Task<PagedResultDto<TagDto>> GetListAsync(TagGetListInput input)
        {
            var tags = await Repository.GetListAsync(input.Filter);
            var count = await Repository.GetCountAsync(input.Filter);

            return new PagedResultDto<TagDto>(
                count,
                ObjectMapper.Map<List<Tag>, List<TagDto>>(tags)
                );
        }

        [Authorize(CmsKitAdminPermissions.Tags.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await Repository.DeleteAsync(id);
        }
    }
}
