using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
//<TEMPLATE-REMOVE IF-NOT='CMS-KIT'>
using Volo.CmsKit;
//</TEMPLATE-REMOVE>

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(MyProjectNameApplicationContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        //<TEMPLATE-REMOVE IF-NOT='CMS-KIT'>
        typeof(CmsKitApplicationModule),
        //</TEMPLATE-REMOVE>
        typeof(AbpSettingManagementApplicationModule)
        )]
    public class MyProjectNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<MyProjectNameApplicationModule>();
            });
        }
    }
}
