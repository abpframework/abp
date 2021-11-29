using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Users
{
    public static class UserLookupServiceExtensions
    {
        public static async Task<TUser> GetByIdAsync<TUser>(this IUserLookupService<TUser> userLookupService, Guid id, CancellationToken cancellationToken = default)
            where TUser : class, IUser
        {
            var user = await userLookupService.FindByIdAsync(id, cancellationToken);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(TUser), id);
            }

            return user;
        }

        public static async Task<TUser> GetByUserNameAsync<TUser>(this IUserLookupService<TUser> userLookupService, string userName, CancellationToken cancellationToken = default)
            where TUser : class, IUser
        {
            var user = await userLookupService.FindByUserNameAsync(userName, cancellationToken);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(TUser), userName);
            }

            return user;
        }
    }
}