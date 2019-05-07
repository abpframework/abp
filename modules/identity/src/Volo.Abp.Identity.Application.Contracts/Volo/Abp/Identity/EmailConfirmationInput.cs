/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.Identity/EmailConfirmationInput
* 创建者：天上有木月
* 创建时间：2019/5/8 1:25:09
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Volo.Abp.Identity
{
    public class EmailConfirmationInput
    {

        /// <summary>
        /// Encrypted user id.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ConfirmationCode { get; set; }
    }
}
