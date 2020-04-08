using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Acme.BookStore.Roles
{
    public class AppRoleAppService_Tests : BookStoreApplicationTestBase
    {
        private readonly IAppRoleAppService _appRoleAppService;

        public AppRoleAppService_Tests()
        {
            _appRoleAppService = GetRequiredService<IAppRoleAppService>();
        }

        [Fact]
        public async Task Should_Query_And_Update_Roles()
        {
            var role = await GetFirstRole();
            role.Title.ShouldBeNull();

            await _appRoleAppService.UpdateTitleAsync(role.Id, "New title!");

            role = await GetFirstRole();
            role.Title.ShouldBe("New title!");
        }

        private async Task<AppRoleDto> GetFirstRole()
        {
            var roles = await _appRoleAppService.GetListAsync();
            return roles.First();
        }
    }
}
