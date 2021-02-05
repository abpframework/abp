using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
    public class EntityTagAdminAppService : CmsKitAdminAppServiceBase, IEntityTagAdminAppService
    {
        protected readonly ITagDefinitionStore _tagDefinitionStore;
        protected readonly IEntityTagManager _entityTagManager;
        protected readonly ITagManager _tagManager;
        protected readonly ITagRepository _tagRepository;
        protected readonly IEntityTagRepository _entityTagRepository;

        public EntityTagAdminAppService(
            ITagDefinitionStore tagDefinitionStore,
            IEntityTagManager entityTagManager,
            ITagManager tagManager,
            ITagRepository tagRepository,
            IEntityTagRepository entityTagRepository)
        {
            _tagDefinitionStore = tagDefinitionStore;
            _entityTagManager = entityTagManager;
            _tagManager = tagManager;
            _tagRepository = tagRepository;
            _entityTagRepository = entityTagRepository;
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

        public async Task SetEntityTagsAsync(EntityTagSetDto input)
        {
            var definition = await _tagDefinitionStore.GetTagEntityTypeDefinitionsAsync(input.EntityType);

            await CheckPolicyAsync(definition.UpdatePolicy);

            var existingTags = await _tagRepository.GetAllRelatedTagsAsync(input.EntityType, input.EntityId, CurrentTenant?.Id);

            var deletedTags = existingTags.Where(x => !input.Tags.Contains(x.Name)).ToList();
            var addedTags = input.Tags.Where(x => !existingTags.Any(a => a.Name == x));

            await _entityTagRepository.DeleteManyAsync(deletedTags.Select(s => s.Id).ToArray());

            foreach (var addedTag in addedTags)
            {
                var tag = await _tagManager.GetOrAddAsync(input.EntityType, addedTag, CurrentTenant?.Id);

                await _entityTagManager.AddTagToEntityAsync(tag.Id, input.EntityType, input.EntityId, CurrentTenant?.Id);
            }            
        }
    }
}
