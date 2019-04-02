/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging/AuditLogActionService
* 创建者：天上有木月
* 创建时间：2019/4/2 2:56:56
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
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions.Dtos;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging
{
    [DisableAuditing]
    [Authorize(AuditLoggingPermissions.AuditLoggs.Default)]
    public class AuditLogActionAppService : ApplicationService, IAuditLogActionAppService
    {
        private readonly IAuditLogActionRepository _auditLogActionRepository;
        public AuditLogActionAppService(IAuditLogActionRepository auditLogActionRepository)
        {
            _auditLogActionRepository = auditLogActionRepository;
        }

        public async Task<ListResultDto<AuditLogActionDto>> GetListAsync(Guid auditId)
        {
            var list = await _auditLogActionRepository.GetListAsync(auditId);

            return new ListResultDto<AuditLogActionDto>(
                ObjectMapper.Map<List<AuditLogAction>, List<AuditLogActionDto>>(list)
            );

        }
    }
}
