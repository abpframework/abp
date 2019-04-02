/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    BaseManagement/BaseItemDTO
* 创建者：天上有木月
* 创建时间：2019/4/3 1:37:08
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace BaseManagement
{
    public class BaseItemDto : EntityDto<Guid>
    {
        public Guid BaseTypeGuid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public int Sort { get; set; }

        public string Remark { get; set; }

    }
}
