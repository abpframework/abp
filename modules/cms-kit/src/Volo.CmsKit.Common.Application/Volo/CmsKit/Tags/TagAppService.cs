using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Tags;

public class TagAppService : CmsKitAppServiceBase, ITagAppService
{
    protected ITagRepository TagRepository { get; }

    public TagAppService(ITagRepository tagRepository)
    {
        TagRepository = tagRepository;
    }

    public virtual async Task<List<TagDto>> GetAllRelatedTagsAsync(string entityType, string entityId)
    {
        var entities = await TagRepository.GetAllRelatedTagsAsync(
            entityType,
            entityId);

        return ObjectMapper.Map<List<Tag>, List<TagDto>>(entities);
    }
}
