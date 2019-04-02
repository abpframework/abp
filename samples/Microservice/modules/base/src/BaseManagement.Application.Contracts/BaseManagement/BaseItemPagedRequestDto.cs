/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    BaseManagement/BaseItemPagedRequestDto
* 创建者：天上有木月
* 创建时间：2019/4/3 1:49:50
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using Volo.Abp.Application.Dtos;

namespace BaseManagement
{
    public class BaseItemPagedRequestDto : PagedAndSortedResultRequestDto
    {
        public Guid? BaseTypeGuid { get; set; }
    }
}
