using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Layout;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IPageLayout), typeof(EmbeddingPageLayout))]
public class EmbeddingPageLayout : PageLayout, IScopedDependency
{
    public bool IsEmbedded { get; protected set; }
    protected IPageEmbeddingService PageEmbeddingService { get; }
    protected IHttpContextAccessor HttpContextAccessor { get; }
    
    private bool? _renderLayoutElements;

    public EmbeddingPageLayout(IPageEmbeddingService pageEmbeddingService, IHttpContextAccessor httpContextAccessor)
    {
        PageEmbeddingService = pageEmbeddingService;
        HttpContextAccessor = httpContextAccessor;
    }

    public override bool RenderLayoutElements 
    { 
        get
        {
            if (_renderLayoutElements.HasValue)
            {
                return _renderLayoutElements.Value;
            }

            // Check if this is an embedding request
            var httpContext = HttpContextAccessor.HttpContext;
            if (httpContext != null && PageEmbeddingService.IsEmbeddingRequest(httpContext))
            {
                IsEmbedded = true;
                return false;
            }

            return true;
        }
        set => _renderLayoutElements = value;
    }
}
