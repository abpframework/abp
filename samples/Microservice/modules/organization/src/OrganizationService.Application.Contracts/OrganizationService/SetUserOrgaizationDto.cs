/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    OrganizationService/SetUserOrgaizationDto
* 创建者：天上有木月
* 创建时间：2019/5/6 14:13:34
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
using Volo.Abp.Users;

namespace OrganizationService
{
    public class SetUserOrgaizationDto
    {
        public Guid  UserId { get; set; }
        public Guid[] OrganizationIds { get; set; }
    }
}
