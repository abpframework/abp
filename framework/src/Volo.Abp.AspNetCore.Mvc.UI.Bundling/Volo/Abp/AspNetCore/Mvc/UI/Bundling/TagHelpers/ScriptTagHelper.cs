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

    [HtmlAttributeName("deferred")]
    public bool? Deferred { get; set; }
    
    public ScriptTagHelper(IOptions<AbpBundlingOptions> options)
    {
        Options = options.Value;
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if(Deferred == true)
        {
            output.Attributes.Add("defer", "");
            return;
        }
        else if (Deferred == false)
        {
            return;
        }
        
        if (Options.DeferScriptsByDefault)
        {
            output.Attributes.Add("defer", "");
        }
        
        var src = output.Attributes["src"]?.Value?.ToString();
        
        if (!src.IsNullOrWhiteSpace() && Options.DeferScripts.Any(x => src.Equals(x, StringComparison.OrdinalIgnoreCase)))
        {
            output.Attributes.Add("defer", "");
        }
    }
}