using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Identity.Blazor.Pages
{
    public partial class UserManagement
    {
        private long _totalUserCount;
        private IReadOnlyList<IdentityUserDto> _users;
        
        private async Task GetUsersAsync()
        {
            var result = await UserAppService.GetListAsync(new GetIdentityUsersInput());
            _totalUserCount = result.TotalCount;
            _users = result.Items;
        }
    }
}
