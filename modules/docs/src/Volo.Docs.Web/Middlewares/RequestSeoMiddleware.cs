using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Docs.Seo;

namespace Volo.Docs.Middlewares
{
    public class RequestSeoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly DocsSeoOptions _docsSeoOptions;
        private readonly DocsSeoBuilder _docsSeoBuilder;

        public RequestSeoMiddleware(
            RequestDelegate next, 
            IOptions<DocsSeoOptions> docsSeoOptions, 
            DocsSeoBuilder docsSeoBuilder
        )
        {
            _next = next;
            _docsSeoBuilder = docsSeoBuilder;
            _docsSeoOptions = docsSeoOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var baseUrl = string.Concat(context.Request.Scheme, "://", context.Request.Host);

            if (context.Request.Path == "/sitemap.xml")
            {
                var sitemap = await GenerateSitemapAsync(baseUrl);

                context.Response.ContentType = "text/xml";
                await context.Response.WriteAsync(sitemap);
            }

            else if (context.Request.Path == "/robots.txt")
            {
                var robotsTxt = GenerateRobotsTxt(baseUrl);
                            
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(robotsTxt);
            }
            
            await _next(context);
        }

        private async Task<string> GenerateSitemapAsync(string baseUrl)
        {
            var links = await _docsSeoBuilder.GetDocumentLinksAsync();
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

            return siteMapBuilder.ToString();
        }

        private string GenerateRobotsTxt(string baseUrl)
        {
            var stringBuilder = new StringBuilder();
                    
            if (_docsSeoOptions.RobotsTxt.Any())
            {
                foreach (var robotsTxt in _docsSeoOptions.RobotsTxt)
                {
                    stringBuilder.AppendLine($"User-agent: {robotsTxt.UserAgent}");
                    
                    if (!robotsTxt.AllowUrls.Any() && !robotsTxt.AllowUrls.Any())
                    {
                        continue;
                    }
                    
                    foreach (var disallowUrl in robotsTxt.DisallowUrls)
                    {
                        stringBuilder.AppendLine($"Disallow: {disallowUrl}");
                    }
                    
                    foreach (var allowUrl in robotsTxt.AllowUrls)
                    {
                        stringBuilder.AppendLine($"Allow: {allowUrl}");
                    }
                }
            }

            stringBuilder.Append($"Sitemap: {baseUrl}/sitemap.xml");
                
            return stringBuilder.ToString();
        }
    }
}