using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.TextTemplating.Razor
{
    public interface IRazorTemplatePage<TModel> : IRazorTemplatePage
    {
        TModel Model { get; set; }
    }

    public interface IRazorTemplatePage
    {
        IServiceProvider ServiceProvider { get; set; }

        IStringLocalizer Localizer { get; set; }

        HtmlEncoder HtmlEncoder { get; set; }

        JavaScriptEncoder JavaScriptEncoder { get; set; }

        UrlEncoder UrlEncoder { get; set; }

        Dictionary<string, object> GlobalContext { get; set; }

        string Body { get; set; }

        Task ExecuteAsync();

        Task<string> GetOutputAsync();
    }
}
