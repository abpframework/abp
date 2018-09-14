using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Volo.Blogging.Users
{
    public interface IBlogUserRepository : IBasicRepository<BlogUser, Guid>, IUserRepository<BlogUser>
    {
        Task<List<BlogUser>> GetUsersAsync(int maxCount, string filter, CancellationToken cancellationToken = default);
    }
}