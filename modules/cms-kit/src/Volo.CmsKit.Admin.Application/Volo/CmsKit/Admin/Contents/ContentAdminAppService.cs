using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Domain.Volo.CmsKit.Contents;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Contents
{
    [Authorize(CmsKitAdminPermissions.Contents.Default)]
    public class ContentAdminAppService :
        CrudAppService<
            Content,
            ContentDto,
            ContentGetListDto,
            Guid,
            ContentGetListInput,
            ContentCreateDto,
            ContentUpdateDto
            >, IContentAdminAppService
    {
        protected IContentManager ContentManager { get; }

        protected IContentRepository ContentRepository { get; }

        public ContentAdminAppService(
            IRepository<Content, Guid> repository,
            IContentManager contentManager,
            IContentRepository contentRepository) : base(repository)
        {
            ContentManager = contentManager;
            ContentRepository = contentRepository;

            GetListPolicyName = CmsKitAdminPermissions.Contents.Default;
            GetPolicyName = CmsKitAdminPermissions.Contents.Default;
            CreatePolicyName = CmsKitAdminPermissions.Contents.Create;
            UpdatePolicyName = CmsKitAdminPermissions.Contents.Update;
            DeletePolicyName = CmsKitAdminPermissions.Contents.Delete;
        }

        [Authorize(CmsKitAdminPermissions.Contents.Create)]
        public override async Task<ContentDto> CreateAsync(ContentCreateDto input)
        {
            var entity = new Content(
                        GuidGenerator.Create(),
                        input.EntityType,
                        input.EntityId,
                        input.Value,
                        CurrentTenant?.Id);

            await ContentManager.InsertAsync(entity);

            return MapToGetOutputDto(entity);
        }

        public async Task<ContentDto> GetByEntityAsync(
            [NotNull] string entityType,
            [NotNull] string entityId)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            var content = await ContentRepository.GetAsync(entityType, entityId, CurrentTenant?.Id);

            return ObjectMapper.Map<Content, ContentDto>(content);
        }

        public async Task SetByEntityAsync(
            [NotNull] string entityType,
            [NotNull] string entityId,
            ContentSetByEntityInput input)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

            var updated = await ContentRepository.FindAsync(entityType, entityId);

            if (updated == null)
            {
                await ContentRepository.InsertAsync(
                    new Content(
                        GuidGenerator.Create(),
                        entityType,
                        entityId,
                        input.Value,
                        CurrentTenant?.Id));
            }
            else
            {
                updated.SetValue(input.Value);
                await ContentRepository.UpdateAsync(updated);
            }
        }
    }
}
