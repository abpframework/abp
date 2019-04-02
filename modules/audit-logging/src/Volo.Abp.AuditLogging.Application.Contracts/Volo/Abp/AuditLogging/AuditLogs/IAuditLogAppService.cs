/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs/IAuditLogAppService
* 创建者：天上有木月
* 创建时间：2019/4/2 2:23:47
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos;

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogInput input);

        Task<AuditLogDto> GetAsync(Guid id);
    }
}
