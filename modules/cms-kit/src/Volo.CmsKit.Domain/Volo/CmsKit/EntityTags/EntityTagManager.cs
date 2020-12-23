using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.EntityTags;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.EntityTags
{
    public class EntityTagManager : IEntityTagManager
    {
        private readonly ITagRepository tagRepository;
        private readonly IEntityTagRepository entityTagRepository;

        public EntityTagManager(
            ITagRepository tagRepository,
            IEntityTagRepository entityTagRepository)
        {
            this.tagRepository = tagRepository;
            this.entityTagRepository = entityTagRepository;
        }

        public Task AddTagToEntityAsync(Guid tagId, string entityType, string entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Tag>> GetEntityTagsAsync(string entityType, string entityId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTagFromEntityAsync(Guid tagId, string entityType, string entityId)
        {
            throw new NotImplementedException();
        }
    }
}
