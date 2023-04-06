using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ControllerScriptCacheItem;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Caching;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.CacheScripts;

public class AbpApplicationLocalizationScriptTagHelperService : AbpTagHelperService<AbpApplicationLocalizationScriptTagHelper>
{
    protected IAbpApplicationLocalizationAppService AbpApplicationLocalizationAppService { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IDistributedCache<AbpControllerScriptCacheItem> Cache { get; }

    public AbpApplicationLocalizationScriptTagHelperService(
        IAbpApplicationLocalizationAppService abpApplicationLocalizationAppService,
        IJsonSerializer jsonSerializer,
        IDistributedCache<AbpControllerScriptCacheItem> cache)
    {
        AbpApplicationLocalizationAppService = abpApplicationLocalizationAppService;
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
        var onlyDynamics = TagHelper.OnlyDynamics ? "&OnlyDynamics={TagHelper.OnlyDynamics}" : "";
        var src = urlHelper.Content($"~/Abp/ApplicationLocalizationScript?cultureName={TagHelper.CultureName}{onlyDynamics}&hash={await GetJsFileHash()}");
        output.Attributes.Add(new TagHelperAttribute("src", new HtmlString(src)));
    }

    protected virtual async Task<string> GetJsFileHash()
    {
        var dto = await AbpApplicationLocalizationAppService.GetAsync(new ApplicationLocalizationRequestDto
        {
            CultureName = TagHelper.CultureName,
            OnlyDynamics = TagHelper.OnlyDynamics
        });

        var json = JsonSerializer.Serialize(dto, indented: true);
        await Cache.SetAsync(nameof(AbpApplicationLocalizationScriptController), new AbpControllerScriptCacheItem(json));
        return $"{json}{TagHelper.CultureName}{TagHelper.OnlyDynamics}".ToMd5().ToLower();
    }
}
