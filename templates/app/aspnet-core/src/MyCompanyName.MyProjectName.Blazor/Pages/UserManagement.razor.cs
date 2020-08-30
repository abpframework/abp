using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Identity;

namespace MyCompanyName.MyProjectName.Blazor.Pages
{
    public partial class UserManagement : ComponentBase
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
