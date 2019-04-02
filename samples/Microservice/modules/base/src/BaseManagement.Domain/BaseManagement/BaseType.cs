/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    BaseManagement/BaseType
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

namespace BaseManagement
{
    public class BaseType: Entity<Guid>
    {
        public BaseType()
        {
        }

        public BaseType(string code, string name)
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

        public virtual ICollection<BaseItem> BaseItems { get; set; }
    }
}
