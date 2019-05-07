/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging/AuditLoggingApplicationContractsModule
* 创建者：天上有木月
* 创建时间：2019/4/2 2:08:22
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using Volo.Abp.Application;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AuditLogging.Application.Contracts.Volo.Abp.AuditLogging
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpAuditLoggingDomainSharedModule))]
    public class AuditLoggingApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AuditLoggingApplicationContractsModule>();
            });
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AuditLoggingResource>("en")
                    .AddVirtualJson("/Volo/Abp/AuditLogging/Localization/ApplicationContracts");
            });
        }
    }
}
