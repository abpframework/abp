using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("Blog/[controller]/[action]")]
    public class PostsController : AbpController
    {
        private readonly IPostAppService _postAppService;

        public PostsController(IPostAppService postAppService)
        {
            _postAppService = postAppService;
        }

        [HttpPost]
        public async Task Delete(Guid id)
        {
            await _postAppService.DeleteAsync(id);
        }
    }
}
