using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Admin.Comments
{
    public class CommentAdminAppService : CmsKitAdminAppServiceBase, ICommentAdminAppService
    {
        protected readonly ICommentRepository CommentRepository;

        public CommentAdminAppService(ICommentRepository commentRepository)
        {
            CommentRepository = commentRepository;
        }

        public virtual Task<PagedResult<CommentDto>> GetListAsync(CommentGetListInput input)
        {
            throw new System.NotImplementedException();
        }

        public virtual async Task<CommentWithAuthorDto> GetAsync(Guid id)
        {
            var comment = await CommentRepository.GetWithAuthorAsync(id);

            var dto = ObjectMapper.Map<Comment, CommentWithAuthorDto>(comment.Comment);
            dto.Author = ObjectMapper.Map<CmsUser, CmsUserDto>(comment.Author);

            return dto;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var comment = await CommentRepository.GetAsync(id);

            await CommentRepository.DeleteWithRepliesAsync(comment);
        }
    }
}