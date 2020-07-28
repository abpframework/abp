using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Comments
{
    [Authorize]
    public class CommentPublicAppService : ApplicationService, ICommentPublicAppService
    {
        protected ICommentRepository CommentRepository { get; }

        public CommentPublicAppService(ICommentRepository commentRepository)
        {
            CommentRepository = commentRepository;
        }

        public async Task<List<CommentWithRepliesDto>> GetAllForEntityAsync(string entityType, string entityId)
        {
            var comments = await CommentRepository.GetListAsync(entityType, entityId);

            return ConvertCommentsToNestedStructure(comments);
        }

        public async Task<CommentDto> CreateAsync(CreateCommentInput input)
        {
            var comment = await CommentRepository.InsertAsync(new Comment(
                GuidGenerator.Create(),
                input.EntityType,
                input.EntityId,
                input.Text,
                input.RepliedCommentId,
                CurrentUser.Id.Value
            ));

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        public async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentInput input)
        {
            var comment = await CommentRepository.GetAsync(id);

            comment.SetText(input.Text);

            var updatedComment = await CommentRepository.UpdateAsync(comment);

            return ObjectMapper.Map<Comment, CommentDto>(updatedComment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await CommentRepository.GetAsync(id);

            if (comment.CreatorId != CurrentUser.Id)
            {
                throw new BusinessException();
            }

            await CommentRepository.DeleteAsync(id);
        }

        private List<CommentWithRepliesDto> ConvertCommentsToNestedStructure(List<Comment> comments)
        {
            var parentComments = comments
                .Where(c=> c.RepliedCommentId == null)
                .Select(c=> ObjectMapper.Map<Comment, CommentWithRepliesDto>(c))
                .ToList();

            foreach (var parentComment in parentComments)
            {
                parentComment.Replies = comments
                    .Where(c => c.RepliedCommentId == parentComment.Id)
                    .Select(c => ObjectMapper.Map<Comment, CommentDto>(c))
                    .ToList();
            }

            return parentComments;
        }
    }
}
