/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.Identity/UserEmailer
* 创建者：天上有木月
* 创建时间：2019/5/8 1:39:35
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Security.Encryption;

namespace Volo.Abp.Identity
{
    public class UserEmailer:DomainService,IUserEmailer
    {
        private readonly IStringEncryptionService _encryptionService;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        public UserEmailer(IStringEncryptionService encryptionService, IEmailTemplateProvider emailTemplateProvider, IEmailSender emailSender)
        {
            _encryptionService = encryptionService;
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
        }

        public async Task SendEmailActivationLinkAsync(IdentityUser user, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new ApplicationException("应该设置电子邮件确认码以发送电子邮件激活链接.");
            }

            var link =  "http://localhost:9000/email-confirm" +
                       "?userId=" + Uri.EscapeDataString(_encryptionService.Encrypt(user.Id.ToString())) +
                       "&confirmationCode=" + Uri.EscapeDataString(user.EmailConfirmationCode);
           var emailConfrimedTemplate= await _emailTemplateProvider.GetAsync(IdentitySettingNames.User.EmailConfirmed);

            var emailTemplate = new StringBuilder(emailConfrimedTemplate.Content);
            emailTemplate.Replace("{EMAIL_TITLE}", "欢迎使用系统");
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", "系统发送此邮件验证您的邮箱地址,并激活您的用户账号.");

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + "姓名" + "</b>: " + user.Name + "<br />");


            mailMessage.AppendLine("<b>" + "用户名" + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + "密码" + "</b>: " + plainPassword + "<br />");
            }

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("请点击以下链接确认您的邮箱地址,并激活您的用户账号:" + "<br /><br />");
            mailMessage.AppendLine("<a href=\"" + link + "\">" + link + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            await _emailSender.SendAsync(user.Email, "激活您的用户账号", emailTemplate.ToString());
        }
    }
}
