using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Comments;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Admin.Comments
{
    [RequiresGlobalFeature(typeof(CommentsFeature))]
    [Authorize(CmsKitAdminPermissions.Comments.Default)]
    public class CommentAdminAppService : CmsKitAdminAppServiceBase, ICommentAdminAppService
    {
        protected ICommentRepository CommentRepository { get; }

        public CommentAdminAppService(ICommentRepository commentRepository)
        {
            CommentRepository = commentRepository;
        }

        public virtual async Task<PagedResultDto<CommentWithAuthorDto>> GetListAsync(CommentGetListInput input)
        {
            var totalCount = await CommentRepository.GetCountAsync(
                input.Text,
                input.EntityType,
                input.RepliedCommentId,
                input.Author,
                input.CreationStartDate,
                input.CreationEndDate);

            var comments = await CommentRepository.GetListAsync(
                input.Text,
                input.EntityType,
                input.RepliedCommentId,
                input.Author,
                input.CreationStartDate,
                input.CreationEndDate,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount
            );

            var dtos = comments.Select(queryResultItem =>
            {
                var dto = ObjectMapper.Map<Comment, CommentWithAuthorDto>(queryResultItem.Comment);
                dto.Author = ObjectMapper.Map<CmsUser, CmsUserDto>(queryResultItem.Author);

                return dto;
            }).ToList();

            return new PagedResultDto<CommentWithAuthorDto>(totalCount, dtos);
        }

        public virtual async Task<CommentWithAuthorDto> GetAsync(Guid id)
        {
            var comment = await CommentRepository.GetWithAuthorAsync(id);

            var dto = ObjectMapper.Map<Comment, CommentWithAuthorDto>(comment.Comment);
            dto.Author = ObjectMapper.Map<CmsUser, CmsUserDto>(comment.Author);

            return dto;
        }

        [Authorize(CmsKitAdminPermissions.Comments.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var comment = await CommentRepository.GetAsync(id);
            await CommentRepository.DeleteWithRepliesAsync(comment);
        }
    }
}