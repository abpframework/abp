/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging/AuditLogAppService
* 创建者：天上有木月
* 创建时间：2019/4/2 2:36:55
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
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos;

namespace Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging
{
    [Authorize(AuditLoggingPermissions.AuditLoggs.Default)]
    [DisableAuditing]
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        public AuditLogAppService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogInput input)
        {
            var count = (int)await _auditLogRepository.GetCountAsync(input.HttpMethod, input.Url, input.UserName, input.ApplicationName, input.CorrelationId, input.MaxExecutionDuration, input.MinExecutionDuration, input.HasException, input.HttpStatusCode);
            var list = await _auditLogRepository.GetListAsync(input.Sorting,input.MaxResultCount, input.SkipCount, input.HttpMethod, input.Url, input.UserName, input.ApplicationName, input.CorrelationId, input.MaxExecutionDuration, input.MinExecutionDuration, input.HasException, input.HttpStatusCode);

            return new PagedResultDto<AuditLogDto>(
                count,
                ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(list)
            );
        }

        public async Task<AuditLogDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<AuditLog, AuditLogDto>(
                await _auditLogRepository.GetAsync(id)
            );
        }
    }
}
