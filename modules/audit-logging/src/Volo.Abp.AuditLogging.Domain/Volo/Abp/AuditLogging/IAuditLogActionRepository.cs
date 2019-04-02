/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.EntityFrameworkCore/IAuditLogActionRepository
* 创建者：天上有木月
* 创建时间：2019/4/2 3:02:02
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AuditLogging
{
    public interface IAuditLogActionRepository : IBasicRepository<AuditLogAction, Guid>
    {
        Task<List<AuditLogAction>> GetListAsync(Guid auditLogId);
    }
}
