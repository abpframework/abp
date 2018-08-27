using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Guids;
using Volo.Blogging.Comments.Dtos;

namespace Volo.Blogging.Comments
{
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

            return hierarchicalComments;
        }

        public async Task<List<CommentDto>> GetListOfPostAsync(GetCommentListOfPostAsync input)
        {
            var comments = await _commentRepository.GetListOfPostAsync(input.PostId);

            return new List<CommentDto>(
                ObjectMapper.Map<List<Comment>, List<CommentDto>>(comments));
        }

        public async Task<CommentDto> CreateAsync(CreateCommentDto input)
        {
            var comment = new Comment(_guidGenerator.Create(), input.PostId, input.RepliedCommentId,input.Text);

            comment = await _commentRepository.InsertAsync(comment);

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        public async Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            var comment = await _commentRepository.GetAsync(id);

            comment.SetText(input.Text);

            comment = await _commentRepository.UpdateAsync(comment);

            return ObjectMapper.Map<Comment, CommentDto>(comment);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _commentRepository.DeleteAsync(id);
        }
    }
}
