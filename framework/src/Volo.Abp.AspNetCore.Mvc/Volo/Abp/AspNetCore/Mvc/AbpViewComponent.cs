using Microsoft.AspNetCore.Mvc;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AspNetCore.Mvc
{
    public abstract class AbpViewComponent : ViewComponent
    {
        public IObjectMapper ObjectMapper { get; set; }

        protected AbpViewComponent()
        {
            
        }
    }
}
