/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions/IAuditLogActionAppService
* 创建者：天上有木月
* 创建时间：2019/4/2 2:26:39
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions.Dtos;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos;

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions
{
    public interface IAuditLogActionAppService:IApplicationService
    {
        Task<ListResultDto<AuditLogActionDto>> GetListAsync(Guid auditId);
    }
}
