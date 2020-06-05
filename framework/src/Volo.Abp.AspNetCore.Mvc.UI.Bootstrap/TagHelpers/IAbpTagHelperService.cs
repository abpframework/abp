using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public interface IAbpTagHelperService<TTagHelper> : ITransientDependency
        where TTagHelper : TagHelper
    {
        TTagHelper TagHelper { get; }

        int Order { get; }

        void Init(TagHelperContext context);

        void Process(TagHelperContext context, TagHelperOutput output);

        Task ProcessAsync(TagHelperContext context, TagHelperOutput output);
    }
}