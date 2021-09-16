// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

// ReSharper disable once CheckNamespace
namespace Volo.Blogging.ClientProxies
{
    public partial class BlogsClientProxy
    {
        public virtual async Task<ListResultDto<BlogDto>> GetListAsync()
        {
            return await RequestAsync<ListResultDto<BlogDto>>(nameof(GetListAsync));
        }

        public virtual async Task<BlogDto> GetByShortNameAsync(string shortName)
        {
            return await RequestAsync<BlogDto>(nameof(GetByShortNameAsync), shortName);
        }

        public virtual async Task<BlogDto> GetAsync(Guid id)
        {
            return await RequestAsync<BlogDto>(nameof(GetAsync), id);
        }
    }
}
