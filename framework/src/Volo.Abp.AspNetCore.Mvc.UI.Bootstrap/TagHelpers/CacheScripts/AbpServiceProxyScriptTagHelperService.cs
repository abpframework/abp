using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.ControllerScriptCacheItem;
using Volo.Abp.AspNetCore.Mvc.ProxyScripting;
using Volo.Abp.Caching;
using Volo.Abp.Http.ProxyScripting;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.CacheScripts;

public class AbpServiceProxyScriptTagHelperService : AbpTagHelperService<AbpServiceProxyScriptTagHelper>
{
    protected IProxyScriptManager ProxyScriptManager { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IDistributedCache<AbpControllerScriptCacheItem> Cache { get; }

    public AbpServiceProxyScriptTagHelperService(
        IProxyScriptManager proxyScriptManager,
        IJsonSerializer jsonSerializer,
        IDistributedCache<AbpControllerScriptCacheItem> cache)
    {
        ProxyScriptManager = proxyScriptManager;
        JsonSerializer = jsonSerializer;
        Cache = cache;
    }

    public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "script";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (TagHelper.Defer)
        {
            output.Attributes.Add(new TagHelperAttribute("defer"));
        }

        var urlHelper = TagHelper.ViewContext.GetUrlHelper();

        var type = TagHelper.Type.IsNullOrWhiteSpace() ? string.Empty : $"type={TagHelper.Type}&";
        var useCache = TagHelper.UseCache ? "" : $"useCache={TagHelper.UseCache}&";
        var modules = TagHelper.Modules.IsNullOrEmpty() ? "" : $"modules={TagHelper.Modules}&";
        var controllers = TagHelper.Controllers.IsNullOrEmpty() ? "" : $"controllers={TagHelper.Controllers}&";
        var actions = TagHelper.Actions.IsNullOrEmpty() ? "" : $"actions={TagHelper.Actions}&";

        var src = urlHelper.Content($"~/Abp/ServiceProxyScript?{type}{useCache}{modules}{controllers}{actions}hash={await GetJsFileHash()}");
        output.Attributes.Add(new TagHelperAttribute("src", new HtmlString(src)));
    }

    protected virtual async Task<string> GetJsFileHash()
    {
        var model = new ServiceProxyGenerationModel
        {
            Type = TagHelper.Type,
            UseCache = TagHelper.UseCache,
            Modules = TagHelper.Modules,
            Controllers = TagHelper.Controllers,
            Actions = TagHelper.Actions
        };
        model.Normalize();
        var script = ProxyScriptManager.GetScript(model.CreateOptions());
        await Cache.SetAsync(nameof(AbpServiceProxyScriptController), new AbpControllerScriptCacheItem(script));
        return $"{script}{TagHelper.Type}{TagHelper.UseCache}{TagHelper.Modules}{TagHelper.Controllers}{TagHelper.Actions}".ToMd5().ToLower();
    }
}
