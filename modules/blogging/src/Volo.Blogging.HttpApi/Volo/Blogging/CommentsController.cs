using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;

namespace Volo.Blogging
{
    [RemoteService]
    [Area("blogging")]
    [Route("api/blogging/comments")]
    public class CommentsController : AbpController, ICommentAppService
    {
        private readonly ICommentAppService _commentAppService;

        public CommentsController(ICommentAppService commentAppService)
        {
            _commentAppService = commentAppService;
        }

        [HttpGet]
        [Route("hierarchical/{postId}")]
        public Task<List<CommentWithRepliesDto>> GetHierarchicalListOfPostAsync(Guid postId)
        {
            return _commentAppService.GetHierarchicalListOfPostAsync(postId);
        }

        [HttpPost]
        public Task<CommentWithDetailsDto> CreateAsync(CreateCommentDto input)
        {
            return _commentAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<CommentWithDetailsDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            return _commentAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
