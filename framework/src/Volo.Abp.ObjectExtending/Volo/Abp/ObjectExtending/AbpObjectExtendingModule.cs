using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Localization;

namespace Volo.Abp.ObjectExtending;

[DependsOn(
    typeof(AbpLocalizationAbstractionsModule),
    typeof(AbpValidationAbstractionsModule)
    )]
public class AbpObjectExtendingModule : AbpModule
{

}
