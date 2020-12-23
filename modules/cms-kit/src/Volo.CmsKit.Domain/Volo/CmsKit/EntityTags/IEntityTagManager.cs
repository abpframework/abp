using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.EntityTags
{
    public interface IEntityTagManager
    {
        Task AddTagToEntityAsync(Guid tagId, string entityType, string entityId);
        Task RemoveTagFromEntityAsync(Guid tagId, string entityType, string entityId);
        Task<IList<Tag>> GetEntityTagsAsync(string entityType, string entityId);
    }
}
