using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Blogs
{
    public interface IBlogAppService : IApplicationService
    {
        Task<PagedResultDto<BlogDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input);

        Task<ListResultDto<BlogDto>> GetListAsync();

        Task<BlogDto> GetByShortNameAsync(string shortName);

        Task<BlogDto> GetAsync(Guid id);

<<<<<<< HEAD
        Task<BlogDto> Create(CreateBlogDto input);

        Task<BlogDto> Update(Guid id, UpdateBlogDto input);

        Task Delete(Guid id);
=======
        Task<BlogDto> Create(CreateBlogDto blog);
>>>>>>> 5023d9202b64c4367e1234ad5e5f7acb76c592e3
    }
}
