using System.Linq;
using System.Linq;
using System.Threading.Tasks;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
    public class EntityTagAdminAppService : CmsKitAdminAppServiceBase, IEntityTagAdminAppService
    {
        protected ITagDefinitionStore TagDefinitionStore { get; }
        protected EntityTagManager EntityTagManager { get; }
        protected TagManager TagManager { get; }
        protected ITagRepository TagRepository { get; }
        protected IEntityTagRepository EntityTagRepository { get; }

        public EntityTagAdminAppService(
            ITagDefinitionStore tagDefinitionStore,
            EntityTagManager entityTagManager,
            TagManager tagManager,
            ITagRepository tagRepository,
            IEntityTagRepository entityTagRepository)
        {
            TagDefinitionStore = tagDefinitionStore;
            EntityTagManager = entityTagManager;
            TagManager = tagManager;
            TagRepository = tagRepository;
            EntityTagRepository = entityTagRepository;
        }

        public virtual async Task AddTagToEntityAsync(EntityTagCreateDto input)
        {
            var definition = await TagDefinitionStore.GetTagEntityTypeDefinitionAsync(input.EntityType);

            await CheckPolicyAsync(definition.CreatePolicy);

            var tag = await TagManager.GetOrAddAsync(input.EntityType, input.TagName);

            await EntityTagManager.AddTagToEntityAsync(
                tag.Id,
                input.EntityType,
                input.EntityId,
                CurrentTenant?.Id);
        }

        public virtual async Task RemoveTagFromEntityAsync(EntityTagRemoveDto input)
        {
            var definition = await TagDefinitionStore.GetTagEntityTypeDefinitionAsync(input.EntityType);

            await CheckPolicyAsync(definition.DeletePolicy);

            await EntityTagManager.RemoveTagFromEntityAsync(
                input.TagId,
                input.EntityType,
                input.EntityId,
                CurrentTenant?.Id);
        }

        public virtual async Task SetEntityTagsAsync(EntityTagSetDto input)
        {
            var definition = await TagDefinitionStore.GetTagEntityTypeDefinitionAsync(input.EntityType);

            await CheckPolicyAsync(definition.UpdatePolicy);

            var existingTags =
                await TagRepository.GetAllRelatedTagsAsync(input.EntityType, input.EntityId);

            var deletedTags = existingTags.Where(x => !input.Tags.Contains(x.Name)).ToList();
            var addedTags = input.Tags.Where(x => !existingTags.Any(a => a.Name == x));

            await EntityTagRepository.DeleteManyAsync(deletedTags.Select(s => s.Id).ToArray());

            foreach (var addedTag in addedTags)
            {
                var tag = await TagManager.GetOrAddAsync(input.EntityType, addedTag);

                await EntityTagManager.AddTagToEntityAsync(tag.Id, input.EntityType, input.EntityId, CurrentTenant?.Id);
            }
        }
    }
}