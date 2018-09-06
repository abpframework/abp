using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Tagging
{
    /* TODO: Custom policy with configuration.
     * We should create a custom policy to see the blog as read only if the blog is
     * configured as 'public' or the current user has the related permission.
     */
    //[Authorize(BloggingPermissions.Tags.Default)]
    public class TagAppService : ApplicationService, ITagAppService
    {
        private readonly ITagRepository _tagRepository;

        public TagAppService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
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

        [Authorize(BloggingPermissions.Tags.Create)]
        public async Task<TagDto> CreateAsync(CreateTagDto input)
        {
            var newTag = await _tagRepository.InsertAsync(
                new Tag(
                    input.Name, 
                    0,
                    input.Description
                    )
                );

            return ObjectMapper.Map<Tag, TagDto>(newTag);
        }

        [Authorize(BloggingPermissions.Tags.Update)]
        public async Task<TagDto> UpdateAsync(Guid id, UpdateTagDto input)
        {
            var tag = await _tagRepository.GetAsync(id);

            tag.SetName(input.Name);
            tag.SetDescription(input.Description);

            tag = await _tagRepository.UpdateAsync(tag);

            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        [Authorize(BloggingPermissions.Tags.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _tagRepository.DeleteAsync(id);
        }

        public async Task<List<TagDto>> GetPopularTags(GetPopularTagsInput input)
        {
            var postTags = (await _tagRepository.GetListAsync()).OrderByDescending(t=>t.UsageCount)
                .WhereIf(input.MinimumPostCount != null, t=>t.UsageCount >= input.MinimumPostCount)
                .Take(input.ResultCount).ToList();


            return new List<TagDto>(
                ObjectMapper.Map<List<Tag>, List<TagDto>>(postTags));
        }
    }
}
