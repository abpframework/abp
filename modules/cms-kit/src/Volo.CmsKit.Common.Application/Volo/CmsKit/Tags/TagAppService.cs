using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.CmsKit.Tags
{
    public class TagAppService : CmsKitAppServiceBase, ITagAppService
    {
        protected readonly ITagManager TagManager;
        protected readonly ITagRepository TagRepository;
        protected readonly IEntityTagRepository EntityTagRepository;

        public TagAppService(
            ITagManager tagManager,
            ITagRepository tagRepository, IEntityTagRepository entityTagRepository)
        {
            TagManager = tagManager;
            TagRepository = tagRepository;
            EntityTagRepository = entityTagRepository;
        }

        public virtual async Task<List<TagDto>> GetAllRelatedTagsAsync(GetRelatedTagsInput input)
        {
            var entities = await TagRepository.GetAllRelatedTagsAsync(
                                                    input.EntityType,
                                                    input.EntityId,
                                                    CurrentTenant.Id);

            if (input.Tags?.Count > 0)
            {
                var nonExisting = input.Tags
                                    .Where(x =>
                                        !entities.Any(a =>
                                                a.Name.Equals(x.Trim(),
                                                StringComparison.InvariantCultureIgnoreCase)));

                foreach (var tag in nonExisting)
                {
                    var insertedTag = await TagManager.GetOrAddAsync(
                                                        input.EntityType,
                                                        tag,
                                                        tenantId: CurrentTenant?.Id);

                    await EntityTagRepository.InsertAsync(
                            new EntityTag(
                                insertedTag.Id,
                                input.EntityId,
                                CurrentTenant?.Id));

                    entities.Add(insertedTag);
                }
            }

            return ObjectMapper.Map<List<Tag>, List<TagDto>>(entities);
        }
    }
}
