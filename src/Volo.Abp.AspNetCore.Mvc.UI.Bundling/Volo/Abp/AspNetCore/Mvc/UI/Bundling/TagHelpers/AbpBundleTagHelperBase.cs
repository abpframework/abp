using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public abstract class AbpBundleTagHelperBase<TTagHelper, TService> : AbpTagHelper<TTagHelper, TService>, IBundleTagHelper
        where TTagHelper : AbpTagHelper<TTagHelper, TService>
        where TService : class, IAbpTagHelperService<TTagHelper>
    {
        public string Name { get; set; }

        protected AbpBundleTagHelperBase(TService service)
            : base(service)
        {

        }

        public virtual string GetNameOrNull()
        {
            return Name;
        }
    }
}