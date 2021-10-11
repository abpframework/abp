using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Blogs
{
    public class BlogAppService : BloggingAppServiceBase, IBlogAppService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogAppService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<ListResultDto<BlogDto>> GetListAsync()
        {
            var blogs = await _blogRepository.GetListAsync();

            return new ListResultDto<BlogDto>(
                ObjectMapper.Map<List<Blog>, List<BlogDto>>(blogs)
            );
        }

        public async Task<BlogDto> GetByShortNameAsync(string shortName)
        {
            Check.NotNullOrWhiteSpace(shortName, nameof(shortName));

            var blog = await _blogRepository.FindByShortNameAsync(shortName);

            if (blog == null)
            {
                throw new EntityNotFoundException(typeof(Blog), shortName);
            }

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        public async Task<BlogDto> GetAsync(Guid id)
        {
            var blog = await _blogRepository.GetAsync(id);

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }
    }
}
