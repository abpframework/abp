using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Comments
{
    [Authorize]
    public class CommentPublicAppService : ApplicationService, ICommentPublicAppService
    {
        protected ICommentRepository CommentRepository { get; }
        public ICmsUserLookupService CmsUserLookupService { get; }

        public CommentPublicAppService(ICommentRepository commentRepository, ICmsUserLookupService cmsUserLookupService)
        {
            CommentRepository = commentRepository;
            CmsUserLookupService = cmsUserLookupService;
        }

        public async Task<ListResultDto<CommentWithDetailsDto>> GetAllForEntityAsync(string entityType, string entityId)
        {
            var commentsWithAuthor = await CommentRepository.GetListAsync(entityType, entityId);

            return new ListResultDto<CommentWithDetailsDto>(
                ConvertCommentsToNestedStructure(commentsWithAuthor)
                );
        }

        public async Task<CommentDto> CreateAsync(CreateCommentInput input)
        {
            var user = await CmsUserLookupService.FindByIdAsync(CurrentUser.Id.Value);

            if (user == null)
            {
                throw new BusinessException(message: "User Not found!");
            }

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

            if (comment.CreatorId != CurrentUser.Id)
            {
                throw new BusinessException();
            }

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

        private List<CommentWithDetailsDto> ConvertCommentsToNestedStructure(List<CommentWithAuthor> comments)
        {
            var parentComments = comments
                .Where(c=> c.Comment.RepliedCommentId == null)
                .Select(c=> ObjectMapper.Map<Comment, CommentWithDetailsDto>(c.Comment))
                .ToList();

            foreach (var parentComment in parentComments)
            {
                parentComment.Author = GetAuthorAsDtoFromCommentList(comments, parentComment.Id);

                parentComment.Replies = comments
                    .Where(c => c.Comment.RepliedCommentId == parentComment.Id)
                    .Select(c => ObjectMapper.Map<Comment, CommentDto>(c.Comment))
                    .ToList();

                foreach (var reply in parentComment.Replies)
                {
                    reply.Author = GetAuthorAsDtoFromCommentList(comments, reply.Id);
                }
            }

            return parentComments;
        }

        private CmsUserDto GetAuthorAsDtoFromCommentList(List<CommentWithAuthor> comments, Guid commentId)
        {
            return ObjectMapper.Map<CmsUser, CmsUserDto>(comments.Single(c => c.Comment.Id == commentId).Author);
        }
    }
}
