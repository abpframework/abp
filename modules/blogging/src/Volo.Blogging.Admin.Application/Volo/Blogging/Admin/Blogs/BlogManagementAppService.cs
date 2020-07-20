using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Blogging.Blogs;

namespace Volo.Blogging.Admin.Blogs
{
    public class BlogManagementAppService : BloggingAdminAppServiceBase, IBlogManagementAppService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogManagementAppService(IBlogRepository blogRepository)
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

        public async Task<BlogDto> GetAsync(Guid id)
        {
            var blog = await _blogRepository.GetAsync(id);

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        [Authorize(BloggingPermissions.Blogs.Create)]
        public async Task<BlogDto> CreateAsync(CreateBlogDto input)
        {
            var newBlog = await _blogRepository.InsertAsync(new Blog(GuidGenerator.Create(), input.Name, input.ShortName)
            {
                Description = input.Description
            });

            return ObjectMapper.Map<Blog, BlogDto>(newBlog);
        }

        [Authorize(BloggingPermissions.Blogs.Update)]
        public async Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input)
        {
            var blog = await _blogRepository.GetAsync(id);

            blog.SetName(input.Name);
            blog.SetShortName(input.ShortName);
            blog.Description = input.Description;

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        [Authorize(BloggingPermissions.Blogs.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _blogRepository.DeleteAsync(id);
        }
    }
}
