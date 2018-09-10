using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;

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

        [HttpPost]
        public async Task Update(Guid id, UpdateCommentDto commentDto)
        {
            await _commentAppService.UpdateAsync(id, commentDto);
        }
    }
}
