using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers;

[HtmlTargetElement("script")]
public class ScriptTagHelper : AbpTagHelper
{
    protected AbpBundlingOptions Options { get; }
    
    [HtmlAttributeName("src")]
    public string Src { get; set; } = default!;
    
    public ScriptTagHelper(IOptions<AbpBundlingOptions> options)
    {
        Options = options.Value;
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Options.DeferScriptsByDefault)
        {
            output.Attributes.Add("defer", "");
        }
        
        if (!Src.IsNullOrWhiteSpace() && Options.DeferScripts.Any(x => Src.Equals(x, StringComparison.OrdinalIgnoreCase)))
        {
            output.Attributes.Add("defer", "");
        }
    }
}