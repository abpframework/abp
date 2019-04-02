/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.HttpApi.Volo.Abp.AuditLogging/AuditLoggingHttpApiModule
* 创建者：天上有木月
* 创建时间：2019/4/2 12:19:33
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging.HttpApi.Volo.Abp.AuditLogging
{
    [DependsOn(
        typeof(AuditLoggingApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AuditLoggingHttpApiModule:AbpModule
    {
    }
}
