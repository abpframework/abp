using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.Identity.ClientProxies
{
    public partial class IdentityUserLookupClientProxy
    {
        public virtual async Task<UserData> FindByIdAsync(Guid id)
        {
            return await RequestAsync<UserData>(nameof(FindByIdAsync), id);
        }

        public virtual async Task<UserData> FindByUserNameAsync(string userName)
        {
            return await RequestAsync<UserData>(nameof(FindByUserNameAsync), userName);
        }

        public virtual async Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
        {
            return await RequestAsync<ListResultDto<UserData>>(nameof(SearchAsync), input);
        }

        public virtual async Task<long> GetCountAsync(UserLookupCountInputDto input)
        {
            return await RequestAsync<long>(nameof(GetCountAsync), input);
        }

    }
}
