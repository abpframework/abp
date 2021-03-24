using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Docs.Documents;
using Volo.Docs.Seo;

namespace Volo.Docs.Controllers
{
    [Route("/")]
    public class SitemapController : Controller
    {
        private readonly IDocumentAppService _documentAppService;
        private readonly DocsSeoOptions _docsSeoOptions;
        
        public SitemapController(IDocumentAppService documentAppService, IOptions<DocsSeoOptions> docsSeoOptions)
        {
            _documentAppService = documentAppService;
            _docsSeoOptions = docsSeoOptions.Value;
        }

        [Route("robots.txt")]
        public ActionResult GetRobotsTxt()
        {
            if (!_docsSeoOptions.IsEnabled)
            {
                return BadRequest();
            }

            var sb = new StringBuilder();

            if (_docsSeoOptions.RobotsTxt.Any())
            {
                foreach (var robotsTxt in _docsSeoOptions.RobotsTxt)
                {
                    sb.AppendLine($"User-agent: {robotsTxt.UserAgent}");

                    if (!robotsTxt.AllowUrls.Any() && !robotsTxt.AllowUrls.Any())
                    {
                        continue;
                    }
                    
                    foreach (var disallowUrl in robotsTxt.DisallowUrls)
                    {
                        sb.AppendLine($"Disallow: {disallowUrl}");
                    }

                    foreach (var allowUrl in robotsTxt.AllowUrls)
                    {
                        sb.AppendLine($"Allow: {allowUrl}");
                    }
                }
            }
            
            sb.Append($"sitemap: {Request.Scheme}://{Request.Host}/sitemap.xml");

            return Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }
        
        [Route("sitemap.xml")]
        public async Task<ActionResult> GetSitemapAsync()
        {
            if (!_docsSeoOptions.IsEnabled)
            {
                return BadRequest();
            }
            
            var baseUrl = string.Concat(Request.Scheme, "://", Request.Host);
            
            var links = await _documentAppService.GetDocumentLinksAsync();
        
            var siteMapBuilder = new SitemapBuilder();
           
            foreach(var link in links)
            {
                siteMapBuilder.AddUrl(baseUrl + link.EnsureStartsWith('/'));
            }

            if (_docsSeoOptions.Sitemap.AdditionalSitemapItems.Any())
            {
                foreach (var sitemap in _docsSeoOptions.Sitemap.AdditionalSitemapItems)
                {
                    siteMapBuilder.AddUrl(baseUrl + sitemap.Url.EnsureStartsWith('/'), sitemap.ModifiedDate, sitemap.Priority);
                }
            }
            
            return Content(siteMapBuilder.ToString(), "text/xml");
        }
    }
}