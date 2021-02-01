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
        protected ITagManager TagManager { get; }

        protected IStringLocalizerFactory StringLocalizerFactory { get; }

        public TagAdminAppService(
            IRepository<Tag, Guid> repository,
            ITagManager tagManager,
            IStringLocalizerFactory stringLocalizerFactory) : base(repository)
        {
            TagManager = tagManager;
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
            var tag = await TagManager.InsertAsync(
                GuidGenerator.Create(),
                input.EntityType,
                input.Name,
                CurrentTenant?.Id);
            
            return MapToGetOutputDto(tag);
        }

        [Authorize(CmsKitAdminPermissions.Tags.Update)]
        public override async Task<TagDto> UpdateAsync(Guid id, TagUpdateDto input)
        {
            var tag = await TagManager.UpdateAsync(
                id,
                input.Name);

            return MapToGetOutputDto(tag);
        }
        protected override IQueryable<Tag> CreateFilteredQuery(TagGetListInput input)
        {
            return base.CreateFilteredQuery(input)
                    .WhereIf(
                        !input.Filter.IsNullOrEmpty(),
                        x =>
                            x.Name.ToLower().Contains(input.Filter) ||
                            x.EntityType.ToLower().Contains(input.Filter));
        }

        public async Task<List<TagDefinitionDto>> GetTagDefinitionsAsync()
        {
            var definitions = await TagManager.GetTagDefinitionsAsync();

            return definitions
                        .Select(s => 
                            new TagDefinitionDto(
                                s.EntityType, 
                                s.DisplayName?.Localize(StringLocalizerFactory) ?? s.EntityType))
                        .ToList();
        }
    }
}
