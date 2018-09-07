using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Comments;

namespace Volo.Blogging.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("Blog/[controller]/[action]")]
    public class CommentsController : AbpController
    {
        private readonly ICommentAppService _commentAppService;

        public CommentsController(ICommentAppService commentAppService)
        {
            _commentAppService = commentAppService;
        }

        [HttpPost]
        public async Task Delete(Guid id)
        {
            await _commentAppService.DeleteAsync(id);
        }
    }
}
