using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public class TagHelperService : ITagHelperService
    {
        public virtual int Order { get; }

        public virtual void Init(TagHelperContext context)
        {
        }

        public virtual void Process(TagHelperContext context, TagHelperOutput output)
        {
        }

        public virtual Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Process(context, output);
            return Task.CompletedTask;
        }
    }
}