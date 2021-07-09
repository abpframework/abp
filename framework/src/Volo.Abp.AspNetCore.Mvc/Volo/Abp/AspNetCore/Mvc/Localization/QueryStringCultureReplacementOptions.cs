using Volo.Abp.Collections;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class AbpQueryStringCultureReplacementOptions
    {
        public ITypeList<IQueryStringCultureReplacementProvider> QueryStringCultureReplacementProviders { get; }

        public AbpQueryStringCultureReplacementOptions()
        {
            QueryStringCultureReplacementProviders = new TypeList<IQueryStringCultureReplacementProvider>();
            QueryStringCultureReplacementProviders.Add<DefaultQueryStringCultureReplacementProvider>();
        }
    }
}
