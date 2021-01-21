using System;
using System.Threading.Tasks;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Application.Volo.CmsKit.Admin.Tags
{
    public class EntityTagAdminAppService : CmsKitAdminAppServiceBase, IEntityTagAdminAppService
    {
        protected ITagDefinitionStore _tagDefinitionStore;
        protected IEntityTagManager _entityTagManager;
        protected ITagManager _tagManager;

        public EntityTagAdminAppService(
            ITagDefinitionStore tagDefinitionStore,
            IEntityTagManager entityTagManager,
            ITagManager tagManager)
        {
            _tagDefinitionStore = tagDefinitionStore;
            _entityTagManager = entityTagManager;
            _tagManager = tagManager;
        }

        public async Task AddTagToEntityAsync(EntityTagCreateDto input)
        {
            var definition = await _tagDefinitionStore.GetTagEntityTypeDefinitionsAsync(input.EntityType);

            await CheckPolicyAsync(definition.CreatePolicy);

            var tag = await _tagManager.GetOrAddAsync(input.EntityType, input.TagName, CurrentTenant?.Id);

            await _entityTagManager.AddTagToEntityAsync(
                tag.Id,
                input.EntityType,
                input.EntityId,
                CurrentTenant?.Id);
        }

        public async Task RemoveTagFromEntityAsync(EntityTagRemoveDto input)
        {
            var definition = await _tagDefinitionStore.GetTagEntityTypeDefinitionsAsync(input.EntityType);

            await CheckPolicyAsync(definition.DeletePolicy);

            await _entityTagManager.RemoveTagFromEntityAsync(
                input.TagId,
                input.EntityType,
                input.EntityId,
                CurrentTenant?.Id);
        }
    }
}
