using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
    [Authorize(CmsKitAdminPermissions.Tags.Default)]
    public class TagAdminAppService :
        CrudAppService<
            Tag,
            TagDto,
            Guid,
            TagGetListInput,
            TagCreateDto,
            TagUpdateDto>,
        ITagAdminAppService
    {
        protected TagManager TagManager { get; }
        protected ITagDefinitionStore TagDefinitionStore { get; }
        protected IStringLocalizerFactory StringLocalizerFactory { get; }

        public TagAdminAppService(
            IRepository<Tag, Guid> repository,
            TagManager tagManager,
            ITagDefinitionStore tagDefinitionStore,
            IStringLocalizerFactory stringLocalizerFactory) : base(repository)
        {
            TagManager = tagManager;
            TagDefinitionStore = tagDefinitionStore;
            StringLocalizerFactory = stringLocalizerFactory;

            GetListPolicyName = CmsKitAdminPermissions.Tags.Default;
            GetPolicyName = CmsKitAdminPermissions.Tags.Default;
            CreatePolicyName = CmsKitAdminPermissions.Tags.Create;
            UpdatePolicyName = CmsKitAdminPermissions.Tags.Update;
            DeletePolicyName = CmsKitAdminPermissions.Tags.Delete;
        }

        [Authorize(CmsKitAdminPermissions.Tags.Create)]
        public override async Task<TagDto> CreateAsync(TagCreateDto input)
        {
            var tag = await TagManager.CreateAsync(
                GuidGenerator.Create(),
                input.EntityType,
                input.Name);

            await Repository.InsertAsync(tag);

            return await MapToGetOutputDtoAsync(tag);
        }

        [Authorize(CmsKitAdminPermissions.Tags.Update)]
        public override async Task<TagDto> UpdateAsync(Guid id, TagUpdateDto input)
        {
            var tag = await TagManager.UpdateAsync(
                id,
                input.Name);

            await Repository.UpdateAsync(tag);

            return await MapToGetOutputDtoAsync(tag);
        }

        protected override async Task<IQueryable<Tag>> CreateFilteredQueryAsync(TagGetListInput input)
        {
            return (await base.CreateFilteredQueryAsync(input))
                    .WhereIf(
                        !input.Filter.IsNullOrEmpty(),
                        x =>
                            x.Name.ToLower().Contains(input.Filter) ||
                            x.EntityType.ToLower().Contains(input.Filter));
        }

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
    }
}
