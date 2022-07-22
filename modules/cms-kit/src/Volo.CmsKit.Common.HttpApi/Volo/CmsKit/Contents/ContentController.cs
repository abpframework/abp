using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Contents;

[RequiresGlobalFeature(typeof(PagesFeature))]
[RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit/contents")]
public class ContentController : Controller, IContentAppService
{
    protected IContentAppService ContentAppService { get; }

    public ContentController(IContentAppService contentAppService)
    {
        ContentAppService = contentAppService;
    }

    [HttpGet]
    [Route("{parseashtml}")]
    public async Task<string> ParseAsHtmlAsync(string content)
    {
        var fragments = await ParseAsync(content);
        return await RenderViewComponent("ContentFragment", new PageDto() { ContentFragments = fragments });
    }
    public virtual async Task<List<ContentFragment>> ParseAsync(string content)
    {
        return await ContentAppService.ParseAsync(content);
    }

    private async Task<string> RenderViewComponent(string viewComponent, object args)
    {
        var sp = HttpContext.RequestServices;

        var helper = new DefaultViewComponentHelper(
            sp.GetRequiredService<IViewComponentDescriptorCollectionProvider>(),
            HtmlEncoder.Default,
            sp.GetRequiredService<IViewComponentSelector>(),
            sp.GetRequiredService<IViewComponentInvokerFactory>(),
            sp.GetRequiredService<IViewBufferScope>());

        using (var writer = new StringWriter())
        {
            var context = new ViewContext(ControllerContext, NullView.Instance, ViewData, TempData, writer, new HtmlHelperOptions());
            helper.Contextualize(context);
            var result = await helper.InvokeAsync(viewComponent, args);
            result.WriteTo(writer, HtmlEncoder.Default);
            await writer.FlushAsync();
            return writer.ToString();
        }
    }

    internal class NullView : IView
    {
        public static readonly NullView Instance = new();

        public string Path => string.Empty;

        public Task RenderAsync(ViewContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return Task.CompletedTask;
        }
    }
}
