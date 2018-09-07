using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Blogging.Comments.Dtos;

namespace Volo.Blogging.Comments
{
    [Authorize(BloggingPermissions.Comments.Default)]
    public class CommentAppService : ApplicationService, ICommentAppService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IGuidGenerator _guidGenerator;

        public CommentAppService(ICommentRepository commentRepository, IGuidGenerator guidGenerator)
        {
            _commentRepository = commentRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<List<CommentWithRepliesDto>> GetHierarchicalListOfPostAsync(GetCommentListOfPostAsync input)
        {
            var comments = await GetListOfPostAsync(input);
            var hierarchicalComments = new List<CommentWithRepliesDto>();

            foreach (var commentDto in comments)
            {
                var parent = hierarchicalComments.Find(c => c.Comment.Id == commentDto.RepliedCommentId);

                if (parent != null)
                {
                    parent.Replies.Add(commentDto);
                }
                else
                {
                    hierarchicalComments.Add(new CommentWithRepliesDto() { Comment = commentDto });
                }
            }

            hierarchicalComments = hierarchicalComments.OrderByDescending(c => c.Comment.CreationTime).ToList();

            foreach (var hierarchicalComment in hierarchicalComments)
            {
                hierarchicalComment.Replies = hierarchicalComment.Replies.OrderBy(c => c.CreationTime).ToList();
            }

            return hierarchicalComments;
        }

        public async Task<List<CommentDto>> GetListOfPostAsync(GetCommentListOfPostAsync input)
        {
            var comments = await _commentRepository.GetListOfPostAsync(input.PostId);

            return new List<CommentDto>(
                ObjectMapper.Map<List<Comment>, List<CommentDto>>(comments));
        }

        [Authorize(BloggingPermissions.Comments.Create)]
        public async Task<CommentDto> CreateAsync(CreateCommentDto input)
        {
            var comment = new Comment(_guidGenerator.Create(), input.PostId, input.RepliedCommentId, input.Text);

            comment = await _commentRepository.InsertAsync(comment);

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        [Authorize(BloggingPermissions.Comments.Update)]
        public async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            var comment = await _commentRepository.GetAsync(id);

            comment.SetText(input.Text);

            comment = await _commentRepository.UpdateAsync(comment);

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await _commentRepository.GetAsync(id);

            if (CurrentUser.Id != comment.CreatorId)
            {
                await DeleteAsAdminAsync(id);
                return;
            }

            await DeleteCommentAsync(id);
        }

        [Authorize(BloggingPermissions.Comments.Delete)]
        private async Task DeleteAsAdminAsync(Guid id)
        {
            await DeleteCommentAsync(id);
        }

        private async Task DeleteCommentAsync(Guid id)
        {
            await _commentRepository.DeleteAsync(id);

            var replies = await _commentRepository.GetRepliesOfComment(id);

            foreach (var reply in replies)
            {
                await _commentRepository.DeleteAsync(reply.Id);
            }
        }
    }
}
