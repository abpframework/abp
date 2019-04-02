/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.HttpApi.Volo.Abp.AuditLogging/AuditLogController
* 创建者：天上有木月
* 创建时间：2019/4/2 11:18:51
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions.Dtos;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos;

namespace Volo.Abp.AuditLogging.HttpApi.Volo.Abp.AuditLogging
{
    [RemoteService]
    [Area("AuditLog")]
    [Route("api/auditLogging/auditLogAction")]
    public class AuditLogActionController: AbpController, IAuditLogActionAppService
    {
        private readonly IAuditLogActionAppService _auditLogAppService;
        public AuditLogActionController(IAuditLogActionAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ListResultDto<AuditLogActionDto>> GetListAsync(Guid  id) 
        {
            return await _auditLogAppService.GetListAsync(id);
        }

    }
}
