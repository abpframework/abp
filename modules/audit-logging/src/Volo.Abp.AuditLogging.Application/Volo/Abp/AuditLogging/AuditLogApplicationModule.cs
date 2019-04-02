/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging/AuditLogApplicationModule
* 创建者：天上有木月
* 创建时间：2019/4/2 1:56:53
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging.Application.Volo.Abp.AuditLogging
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule))]
    public class AuditLogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AuditLoggingApplicationModuleAutoMapperProfile>(validate: true);
            });



            Configure<AuthorizationOptions>(options =>
            {
                //TODO: Rename UpdatePolicy/DeletePolicy since it's candidate to conflicts with other modules!
            });

        }
    }
}
