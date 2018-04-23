using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public interface ITagHelperService : ITransientDependency
    {
        int Order { get; }

        void Init(TagHelperContext context);

        Task ProcessAsync(TagHelperContext context, TagHelperOutput output);
    }
}