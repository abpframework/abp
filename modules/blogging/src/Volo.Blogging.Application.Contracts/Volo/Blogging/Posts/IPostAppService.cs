using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Blogging.Posts
{
    public interface IPostAppService : IApplicationService
    {
        ListResultDto<PostDto> GetPostsByBlogId(Guid id);

        Task<PostDto> GetPost(GetPostInput input);
    }
}
