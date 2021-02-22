using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    [RequiresGlobalFeature(typeof(MediaFeature))]
    [Authorize(CmsKitAdminPermissions.MediaDescriptors.Default)]
    public class MediaDescriptorAdminAppService : CmsKitAdminAppServiceBase, IMediaDescriptorAdminAppService
    {
        protected readonly IBlobContainer<MediaContainer> MediaContainer;
        protected readonly IMediaDescriptorRepository MediaDescriptorRepository;
        
        public MediaDescriptorAdminAppService(IBlobContainer<MediaContainer> mediaContainer, IMediaDescriptorRepository mediaDescriptorRepository)
        {
            MediaContainer = mediaContainer;
            MediaDescriptorRepository = mediaDescriptorRepository;
        }

        [Authorize(CmsKitAdminPermissions.MediaDescriptors.Create)]
        public virtual async Task<MediaDescriptorDto> CreateAsync(CreateMediaInputStream inputStream)
        {
            var newId = GuidGenerator.Create();
            var stream = inputStream.GetStream();
            var newEntity = new MediaDescriptor(newId, inputStream.Name, inputStream.ContentType, stream.Length, CurrentTenant.Id);

            await MediaContainer.SaveAsync(newId.ToString(), stream);
            await MediaDescriptorRepository.InsertAsync(newEntity);

            return ObjectMapper.Map<MediaDescriptor, MediaDescriptorDto>(newEntity);
        }

        [Authorize(CmsKitAdminPermissions.MediaDescriptors.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await MediaContainer.DeleteAsync(id.ToString());
            await MediaDescriptorRepository.DeleteAsync(id);
        }
    }
}