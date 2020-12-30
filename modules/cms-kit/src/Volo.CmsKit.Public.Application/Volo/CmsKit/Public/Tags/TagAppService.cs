using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Public.Tags
{
    public class TagAppService : CmsKitAppServiceBase, ITagAppService
    {
        protected readonly ITagManager TagManager;
        protected readonly ITagRepository TagRepository;
        protected readonly IEntityTagRepository EntityTagRepository;

        public TagAppService(
            ITagManager tagManager,
            ITagRepository tagRepository,
            IEntityTagRepository entityTagRepository)
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

            return ObjectMapper.Map<List<Tag>, List<TagDto>>(entities);
        }
    }
}
