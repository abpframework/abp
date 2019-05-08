/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.Identity/UserEmail_Tests
* 创建者：天上有木月
* 创建时间：2019/5/8 13:56:39
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.Identity
{
    public class UserEmail_Tests : AbpIdentityDomainTestBase
    {
        private readonly IUserEmailer _userEmailer;
        public UserEmail_Tests()
        {
            _userEmailer= GetRequiredService<IUserEmailer>();
        }

        [Fact]
        public  async  Task SendEmailActivationLinkAsync()
        {
            var identityUser = new IdentityUser(Guid.NewGuid(), "admin11", "710277267@qq.com");
            identityUser.SetNewEmailConfirmationCode();
            await _userEmailer.SendEmailActivationLinkAsync(identityUser,   "1233qwe");
        }
    }
}
