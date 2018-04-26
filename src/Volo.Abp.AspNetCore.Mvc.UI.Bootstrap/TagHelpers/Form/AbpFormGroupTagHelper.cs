using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpFormGroupTagHelper : AbpTagHelper<AbpFormGroupTagHelper, AbpFormGroupTagHelperService>
    {
        
        public bool Checkbox { get; set; }

        public AbpFormGroupTagHelper(AbpFormGroupTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
