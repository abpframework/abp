using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Guids;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;
using Volo.Blogging.Users;

namespace Volo.Blogging.Comments
{
    public class CommentAppService : BloggingAppServiceBase, ICommentAppService
    {
        protected IBlogUserLookupService UserLookupService;

        private readonly ICommentRepository _commentRepository;
        private readonly IGuidGenerator _guidGenerator;

        public CommentAppService(ICommentRepository commentRepository, IGuidGenerator guidGenerator, IBlogUserLookupService userLookupService)
        {
            _commentRepository = commentRepository;
            _guidGenerator = guidGenerator;
            UserLookupService = userLookupService;
        }

        public async Task<List<CommentWithRepliesDto>> GetHierarchicalListOfPostAsync(Guid postId)
        {
            var comments = await GetListOfPostAsync(postId);
            var userDictionary = new Dictionary<Guid, BlogUserDto>();

            foreach (var commentDto in comments)
            {
                if (commentDto.CreatorId.HasValue)
                {
                    var creatorUser = await UserLookupService.FindByIdAsync(commentDto.CreatorId.Value);

                    if (creatorUser != null && !userDictionary.ContainsKey(creatorUser.Id))
                    {
                        userDictionary.Add(creatorUser.Id, ObjectMapper.Map<BlogUser, BlogUserDto>(creatorUser));
                    }
                }
            }

            foreach (var commentDto in comments)
            {
                if (commentDto.CreatorId.HasValue && userDictionary.ContainsKey((Guid)commentDto.CreatorId))
                {
                    commentDto.Writer = userDictionary[(Guid)commentDto.CreatorId];
                }
            }

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

            return hierarchicalComments;
        }

        private async Task<List<CommentWithDetailsDto>> GetListOfPostAsync(Guid postId)
        {
            var comments = await _commentRepository.GetListOfPostAsync(postId);

            return new List<CommentWithDetailsDto>(
                ObjectMapper.Map<List<Comment>, List<CommentWithDetailsDto>>(comments));
        }

        [Authorize]
        public async Task<CommentWithDetailsDto> CreateAsync(CreateCommentDto input)
        {
            var comment = new Comment(_guidGenerator.Create(), input.PostId, input.RepliedCommentId, input.Text);

            comment = await _commentRepository.InsertAsync(comment);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Comment, CommentWithDetailsDto>(comment);
        }

        [Authorize]
        public async Task<CommentWithDetailsDto> UpdateAsync(Guid id, UpdateCommentDto input)
        {
            var comment = await _commentRepository.GetAsync(id);

            await AuthorizationService.CheckAsync(comment, CommonOperations.Update);

            comment.SetText(input.Text);

            comment = await _commentRepository.UpdateAsync(comment);

            return ObjectMapper.Map<Comment, CommentWithDetailsDto>(comment);
        }

        [Authorize]
        public async Task DeleteAsync(Guid id)
        {
            var comment = await _commentRepository.GetAsync(id);

            await AuthorizationService.CheckAsync(comment, CommonOperations.Delete);

            await _commentRepository.DeleteAsync(id);

            var replies = await _commentRepository.GetRepliesOfComment(id);

            foreach (var reply in replies)
            {
                await _commentRepository.DeleteAsync(reply.Id);
            }
        }
    }
}
