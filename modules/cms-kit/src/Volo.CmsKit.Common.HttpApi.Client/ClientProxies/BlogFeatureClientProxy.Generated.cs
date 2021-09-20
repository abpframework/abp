// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Blogs.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IBlogFeatureAppService), typeof(BlogFeatureClientProxy))]
    public partial class BlogFeatureClientProxy : ClientProxyBase<IBlogFeatureAppService>, IBlogFeatureAppService
    {
        public virtual async Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName)
        {
            return await RequestAsync<BlogFeatureDto>(nameof(GetOrDefaultAsync), blogId, featureName);
        }
    }
}
