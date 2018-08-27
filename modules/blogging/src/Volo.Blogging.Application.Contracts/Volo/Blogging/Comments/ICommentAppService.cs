using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Blogging.Comments.Dtos;

namespace Volo.Blogging.Comments
{
    public interface ICommentAppService : IApplicationService
    {
        Task<List<CommentDto>> GetListOfPostAsync(GetCommentListOfPostAsync input);

        Task<List<CommentWithRepliesDto>> GetHierarchicalListOfPostAsync(GetCommentListOfPostAsync input);

        Task<CommentDto> CreateAsync(CreateCommentDto input);

        Task<CommentDto> UpdateAsync(Guid id, UpdateCommentDto input);

        Task DeleteAsync(Guid id);
    }
}
