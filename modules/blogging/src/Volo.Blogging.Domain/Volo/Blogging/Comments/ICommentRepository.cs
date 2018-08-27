using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Comments
{
    public interface ICommentRepository : IBasicRepository<Comment, Guid>
    {
        Task<List<Comment>> GetListOfPostAsync(
            Guid postId
        );
    }
}
