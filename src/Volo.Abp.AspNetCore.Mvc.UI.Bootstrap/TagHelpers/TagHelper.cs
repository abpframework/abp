using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public abstract class TagHelper<TService> : TagHelper
        where TService : ITagHelperService
    {
        private readonly TService _tagHelperService;

        protected TagHelper(TService tagHelperService)
        {
            _tagHelperService = tagHelperService;
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return _tagHelperService.ProcessAsync(context, output);
        }
    }
}