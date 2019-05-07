/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.Identity/IUserEmailer
* 创建者：天上有木月
* 创建时间：2019/5/8 1:37:13
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System.Threading.Tasks;

namespace Volo.Abp.Identity
{
    public interface IUserEmailer
    {
        /// <summary>
        /// 发送电子邮件激活链接到用户的电子邮件地址。
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="plainPassword">
        /// 可以设置为用户的普通密码，将其包含在电子邮件中。
        /// </param>
        Task SendEmailActivationLinkAsync(IdentityUser user, string plainPassword = null);
    }
}
