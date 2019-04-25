/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    OrganizationService/UserOrganization
* 创建者：天上有木月
* 创建时间：2019/4/2 21:16:20
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrganizationService
{
    public class UserOrganization : Entity<Guid>
    {
        private UserOrganization()
        {
        }
        public Guid UserId{get;set;}

        public Guid OrganizationId{get;set;}

        public virtual Organization Organization { get; set; }
    }
}
