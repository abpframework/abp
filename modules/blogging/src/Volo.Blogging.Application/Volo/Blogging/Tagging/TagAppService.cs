using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Tagging
{
    public class TagAppService : ApplicationService, ITagAppService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IPostTagRepository _postTagRepository;

        public TagAppService(ITagRepository tagRepository, IPostTagRepository postTagRepository)
        {
            _tagRepository = tagRepository;
            _postTagRepository = postTagRepository;
        }

        public async Task<List<TagDto>> GetListAsync()
        {
            var tags = await _tagRepository.GetListAsync();

            return new List<TagDto>(
                ObjectMapper.Map<List<Tag>, List<TagDto>>(tags));
        }

        public async Task<List<TagDto>> GetListAsync(GetTagListInput input)
        {
            var tags = await _tagRepository.GetListAsync(input.Ids);

            return new List<TagDto>(
                ObjectMapper.Map<List<Tag>, List<TagDto>>(tags));
        }

        public async Task<TagDto> CreateAsync(CreateTagDto input)
        {
            var newTag = await _tagRepository.InsertAsync(
                new Tag(
                    input.Name,
                    input.Description
                    )
                );

            return ObjectMapper.Map<Tag, TagDto>(newTag);
        }

        public async Task<TagDto> UpdateAsync(Guid id, UpdateTagDto input)
        {
            var tag = await _tagRepository.GetAsync(id);

            tag.SetName(input.Name);
            tag.SetDescription(input.Description);

            tag = await _tagRepository.UpdateAsync(tag);

            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _tagRepository.DeleteAsync(id);
        }

        public async Task<List<PopularTagDto>> GetPopularTags(GetPopularTagsInput input)
        {
            var postTags = await _postTagRepository.GetListAsync();

            var postTagsGrouped = postTags.GroupBy(x => x.PostId)
                .OrderByDescending(g => g.Count())
                .SelectMany(g => g).Select(t=>t.TagId).Distinct().ToList();

            var popularTagDtos = new List<PopularTagDto>();

            for (var i = 0; i < input.ResultCount && i < postTagsGrouped.Count; i++)
            {
                var tagId = postTagsGrouped[i];
                var count = postTags.Count(t => t.TagId == tagId);
                if (count < input.MinimumPostCount)
                {
                    break;
                }
                var tag = await _tagRepository.GetAsync(tagId);

                popularTagDtos.Add(new PopularTagDto
                {
                    Tag = ObjectMapper.Map<Tag, TagDto>(tag),
                    Count = count
                });
            }

            return popularTagDtos;
        }
    }
}
