using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Posts
{
    public interface IPostTagRepository : IBasicRepository<PostTag>
    {
        void DeleteOfPost(Guid id);
    }
}
