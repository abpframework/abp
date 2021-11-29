using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public abstract class AbpTagHelper : TagHelper, ITransientDependency
    {

    }

    public abstract class AbpTagHelper<TTagHelper, TService> : AbpTagHelper
        where TTagHelper : AbpTagHelper<TTagHelper, TService>
        where TService : class, IAbpTagHelperService<TTagHelper>
    {
        protected TService Service { get; }

        public override int Order => Service.Order;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected AbpTagHelper(TService service)
        {
            Service = service;
            Service.As<AbpTagHelperService<TTagHelper>>().TagHelper = (TTagHelper)this;
        }

        public override void Init(TagHelperContext context)
        {
            Service.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Service.Process(context, output);
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return Service.ProcessAsync(context, output);
        }
    }
}
