using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Tagging
{
    public class TagAppService : BloggingAppServiceBase, ITagAppService
    {
        private readonly ITagRepository _tagRepository;

        public TagAppService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<TagDto>> GetPopularTagsAsync(Guid blogId, GetPopularTagsInput input)
        {
            var postTags = (await _tagRepository.GetListAsync(blogId)).OrderByDescending(t=>t.UsageCount)
                .WhereIf(input.MinimumPostCount != null, t=>t.UsageCount >= input.MinimumPostCount)
                .Take(input.ResultCount).ToList();

            return new List<TagDto>(
                ObjectMapper.Map<List<Tag>, List<TagDto>>(postTags));
        }
    }
}
