using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.Docs.Documents;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Volo.Docs.Documents.ClientProxies
{
    public partial class DocsDocumentClientProxy
    {
        public virtual async Task<DocumentWithDetailsDto> GetAsync(GetDocumentInput input)
        {
            return await RequestAsync<DocumentWithDetailsDto>(nameof(GetAsync), input);
        }
 
        public virtual async Task<DocumentWithDetailsDto> GetDefaultAsync(GetDefaultDocumentInput input)
        {
            return await RequestAsync<DocumentWithDetailsDto>(nameof(GetDefaultAsync), input);
        }
 
        public virtual async Task<NavigationNode> GetNavigationAsync(GetNavigationDocumentInput input)
        {
            return await RequestAsync<NavigationNode>(nameof(GetNavigationAsync), input);
        }
 
        public virtual async Task<DocumentResourceDto> GetResourceAsync(GetDocumentResourceInput input)
        {
            return await RequestAsync<DocumentResourceDto>(nameof(GetResourceAsync), input);
        }
 
        public virtual async Task<List<DocumentSearchOutput>> SearchAsync(DocumentSearchInput input)
        {
            return await RequestAsync<List<DocumentSearchOutput>>(nameof(SearchAsync), input);
        }
 
        public virtual async Task<bool> FullSearchEnabledAsync()
        {
            return await RequestAsync<bool>(nameof(FullSearchEnabledAsync));
        }
 
        public virtual async Task<List<String>> GetUrlsAsync(string prefix)
        {
            return await RequestAsync<List<String>>(nameof(GetUrlsAsync), prefix);
        }
 
        public virtual async Task<DocumentParametersDto> GetParametersAsync(GetParametersDocumentInput input)
        {
            return await RequestAsync<DocumentParametersDto>(nameof(GetParametersAsync), input);
        }
 
    }
}
