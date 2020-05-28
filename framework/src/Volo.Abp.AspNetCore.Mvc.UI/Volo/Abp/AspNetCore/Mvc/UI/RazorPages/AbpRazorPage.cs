using System;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Volo.Abp.AspNetCore.Mvc.UI.RazorPages
{
    public abstract class AbpRazorPage<TModel> : RazorPage<TModel>
    {
        public string ApplicationPath
        {
            get
            {
                string str = ViewContext.HttpContext.Request.PathBase.Value;
                return str == null ? "/" : str.EnsureEndsWith('/');
            }
        }
    }
}
