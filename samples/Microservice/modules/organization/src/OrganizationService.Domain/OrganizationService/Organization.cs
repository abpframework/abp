/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    OrganizationService/BaseType
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
    public class Organization : Entity<Guid>
    {
        public Organization()
        {
        }

        public Organization(string code, string name)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Guid? ParentId { get; set; }
        [NotNull]
        public string Code { get; set; }
        [NotNull]
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }


    }
}
