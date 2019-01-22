using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;

namespace Volo.Blogging
{
    [RemoteService]
    [Area("blogging")]
    [Controller]
    [ControllerName("Comments")]
    [Route("api/blogging/comments")]
    [DisableAuditing]
    public class CommentsController : AbpController, ICommentAppService
    {
        [HttpGet]
        [Route("{postId}")]
        public Task<List<CommentWithRepliesDto>> GetHierarchicalListOfPostAsync(Guid postId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<CommentWithDetailsDto> CreateAsync(CreateCommentDto input)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{id}")]
        public Task<CommentWithDetailsDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
