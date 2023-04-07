using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ControllerScriptCacheItem;
using Volo.Abp.Caching;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.CacheScripts;

public class AbpApplicationConfigurationScriptTagHelperService : AbpTagHelperService<AbpApplicationConfigurationScriptTagHelper>
{
    protected IAbpApplicationConfigurationAppService AbpApplicationConfigurationAppService { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IDistributedCache<AbpControllerScriptCacheItem> Cache { get; }

    public AbpApplicationConfigurationScriptTagHelperService(
        IAbpApplicationConfigurationAppService abpApplicationConfigurationAppService,
        IJsonSerializer jsonSerializer,
        IDistributedCache<AbpControllerScriptCacheItem> cache)
    {
        AbpApplicationConfigurationAppService = abpApplicationConfigurationAppService;
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
        var includeLocalizationResources = TagHelper.IncludeLocalizationResources ? $"?IncludeLocalizationResources={TagHelper.IncludeLocalizationResources}&" : "?";
        var src = urlHelper.Content($"~/Abp/ApplicationConfigurationScript{includeLocalizationResources}hash={await GetJsFileHash()}");
        output.Attributes.Add(new TagHelperAttribute("src", new HtmlString(src)));
    }

    protected virtual async Task<string> GetJsFileHash()
    {
        var dto = await AbpApplicationConfigurationAppService.GetAsync(new ApplicationConfigurationRequestOptions
        {
            IncludeLocalizationResources = TagHelper.IncludeLocalizationResources
        });

        var json = JsonSerializer.Serialize(dto, indented: true);
        await Cache.SetAsync(nameof(AbpApplicationConfigurationScriptController), new AbpControllerScriptCacheItem(json));
        return $"{json}{TagHelper.IncludeLocalizationResources}".ToMd5().ToLower();
    }
}
