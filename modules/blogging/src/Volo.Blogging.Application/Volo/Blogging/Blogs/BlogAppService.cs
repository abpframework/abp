using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Blogging.Blogs
{
    public class BlogAppService : ApplicationService, IBlogAppService
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
    }
}
