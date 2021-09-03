using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Admin.MediaDescriptors;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.MediaDescriptors.ClientProxies
{
    public partial class MediaDescriptorAdminClientProxy
    {
        public virtual async Task<MediaDescriptorDto> CreateAsync(string entityType, CreateMediaInputWithStream inputStream)
        {
            return await RequestAsync<MediaDescriptorDto>(nameof(CreateAsync), entityType, inputStream);
        }
 
        public virtual async Task DeleteAsync(Guid id)
        {
            await RequestAsync(nameof(DeleteAsync), id);
        }
 
    }
}
