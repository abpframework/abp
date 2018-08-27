using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Tagging
{
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
    }
}
