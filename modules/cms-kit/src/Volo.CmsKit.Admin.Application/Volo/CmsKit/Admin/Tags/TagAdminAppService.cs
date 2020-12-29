using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
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

        public TagAdminAppService(
            IRepository<Tag, Guid> repository,
            ITagManager tagManager) : base(repository)
        {
            TagManager = tagManager;

            GetListPolicyName = CmsKitAdminPermissions.Tags.Default;
            GetPolicyName = CmsKitAdminPermissions.Tags.Default;
            CreatePolicyName = CmsKitAdminPermissions.Tags.Create;
            UpdatePolicyName = CmsKitAdminPermissions.Tags.Update;
            DeletePolicyName = CmsKitAdminPermissions.Tags.Delete;
        }

        [Authorize(CmsKitAdminPermissions.Tags.Create)]
        public override async Task<TagDto> CreateAsync(TagCreateDto input)
        {
            var inserted = await TagManager.InsertAsync(
                GuidGenerator.Create(),
                input.EntityType,
                input.Name,
                CurrentTenant?.Id);
            return MapToGetOutputDto(inserted);
        }

        [Authorize(CmsKitAdminPermissions.Tags.Update)]
        public override async Task<TagDto> UpdateAsync(Guid id, TagUpdateDto input)
        {
            var updated = await TagManager.UpdateAsync(
                id,
                input.Name);

            return MapToGetOutputDto(updated);
        }
        protected override IQueryable<Tag> CreateFilteredQuery(TagGetListInput input)
        {
            return base.CreateFilteredQuery(input)
                    .WhereIf(
                        !string.IsNullOrEmpty(input.Filter),
                        x =>
                            x.Name.ToLower().Contains(input.Filter) ||
                            x.EntityType.ToLower().Contains(input.Filter));
        }
    }
}
