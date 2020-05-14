using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [Dependency(TryRegister = true)]
    public class HttpClientExternalUserLookupServiceProvider : IExternalUserLookupServiceProvider, ITransientDependency
    {
        protected IIdentityUserLookupAppService UserLookupAppService { get; }

        public HttpClientExternalUserLookupServiceProvider(IIdentityUserLookupAppService userLookupAppService)
        {
            UserLookupAppService = userLookupAppService;
        }

        public virtual async Task<IUserData> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await UserLookupAppService.FindByIdAsync(id);
        }

        public virtual async Task<IUserData> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await UserLookupAppService.FindByUserNameAsync(userName);
        }

        public async Task<List<IUserData>> SearchAsync(
            string sorting,
            string filter, 
            int maxResultCount, 
            CancellationToken cancellationToken = default)
        {
            var result = await UserLookupAppService.SearchAsync(
                new UserLookupSearchInputDto
                {
                    Filter = filter,
                    MaxResultCount = maxResultCount
                }
            );

            return result.Items.Cast<IUserData>().ToList();
        }
    }
}
