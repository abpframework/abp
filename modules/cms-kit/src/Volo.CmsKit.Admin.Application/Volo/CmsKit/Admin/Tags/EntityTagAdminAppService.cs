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
            var definition = await TagDefinitionStore.GetAsync(input.EntityType);

            await CheckAnyOfPoliciesAsync(definition.CreatePolicies);

            var tag = await TagManager.GetOrAddAsync(input.EntityType, input.TagName);

            await EntityTagManager.AddTagToEntityAsync(
                tag.Id,
                input.EntityType,
                input.EntityId,
                CurrentTenant?.Id);
        }

        public virtual async Task RemoveTagFromEntityAsync(EntityTagRemoveDto input)
        {
            var definition = await TagDefinitionStore.GetAsync(input.EntityType);

            await CheckAnyOfPoliciesAsync(definition.DeletePolicies);

            await EntityTagManager.RemoveTagFromEntityAsync(
                input.TagId,
                input.EntityType,
                input.EntityId,
                CurrentTenant?.Id);
        }

        public virtual async Task SetEntityTagsAsync(EntityTagSetDto input)
        {
            var definition = await TagDefinitionStore.GetAsync(input.EntityType);

            await CheckAnyOfPoliciesAsync(definition.UpdatePolicies);

            await EntityTagManager.SetEntityTagsAsync(input.EntityType, input.EntityId, input.Tags);
        }
    }
}
