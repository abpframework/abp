using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Public.Pages;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Pages.ClientProxies
{
    public partial class PagesPublicClientProxy
    {
        public virtual async Task<PageDto> FindBySlugAsync(string slug)
        {
            return await RequestAsync<PageDto>(nameof(FindBySlugAsync), slug);
        }
 
    }
}
