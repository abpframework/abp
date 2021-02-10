using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
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
            var newEntity = new MediaDescriptor(newId, CurrentTenant.Id, inputStream.Name, inputStream.ContentType, stream.Length);

            await MediaDescriptorRepository.InsertAsync(newEntity);
            await MediaContainer.SaveAsync(newId.ToString(), stream);

            return ObjectMapper.Map<MediaDescriptor, MediaDescriptorDto>(newEntity);
        }

        [Authorize(CmsKitAdminPermissions.MediaDescriptors.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await MediaContainer.DeleteAsync(id.ToString());
            await MediaDescriptorRepository.DeleteAsync(id);
        }

        [AllowAnonymous]
        public virtual async Task<RemoteStreamContent> DownloadAsync(Guid id, GetMediaRequestDto request)
        {
            var entity = await MediaDescriptorRepository.GetAsync(id);
            var stream = await MediaContainer.GetAsync(id.ToString());

            return new RemoteStreamContent(stream)
            {
                ContentType = entity.MimeType
            };
        }
    }
}