using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Public.Blogs;

namespace Volo.CmsKit.Public.HttpApi.Volo.CmsKit.Public.Blogs
{
    public class BlogFeaturePublicController : CmsKitPublicControllerBase, IBlogFeaturePublicAppService
    {
        protected IBlogFeaturePublicAppService BlogFeaturePublicAppService { get; }

        public BlogFeaturePublicController(IBlogFeaturePublicAppService blogFeaturePublicAppService)
        {
            BlogFeaturePublicAppService = blogFeaturePublicAppService;
        }

        [HttpGet]
        [Route("{blogId}/{featureName}")]
        public Task<BlogFeatureDto> GetAsync(Guid blogId, string featureName)
        {
            return BlogFeaturePublicAppService.GetAsync(blogId, featureName);
        }
    }
}
