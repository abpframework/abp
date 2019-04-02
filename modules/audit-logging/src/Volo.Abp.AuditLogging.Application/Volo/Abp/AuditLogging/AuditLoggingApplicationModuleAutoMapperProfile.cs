/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging/AuditLoggingApplicationModuleAutoMapperProfile
* 创建者：天上有木月
* 创建时间：2019/4/2 2:47:20
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using AutoMapper;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogActions.Dtos;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging.AuditLogs.Dtos;

namespace Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging
{
    public class AuditLoggingApplicationModuleAutoMapperProfile : Profile
    {
        public AuditLoggingApplicationModuleAutoMapperProfile()
        {
            CreateMap<AuditLog, AuditLogDto>();
            CreateMap<AuditLogAction, AuditLogActionDto>();
        }
    }
}
