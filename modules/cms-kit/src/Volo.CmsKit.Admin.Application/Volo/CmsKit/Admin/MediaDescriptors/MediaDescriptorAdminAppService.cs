using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.CmsKit.MediaDescriptors;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    [Authorize]
    public class MediaDescriptorAdminAppService : CmsKitAdminAppServiceBase, IMediaDescriptorAdminAppService
    {
        protected readonly IBlobContainer<MediaContainer> MediaContainer;
        protected readonly IMediaDescriptorRepository MediaDescriptorRepository;
        
        public MediaDescriptorAdminAppService(IBlobContainer<MediaContainer> mediaContainer, IMediaDescriptorRepository mediaDescriptorRepository)
        {
            MediaContainer = mediaContainer;
            MediaDescriptorRepository = mediaDescriptorRepository;
        }

        public virtual async Task<MediaDescriptorDto> GetAsync(Guid id)
        {
            var entity = await MediaDescriptorRepository.GetAsync(id);

            return ObjectMapper.Map<MediaDescriptor, MediaDescriptorDto>(entity);
        }

        public virtual async Task<PagedResultDto<MediaDescriptorGetListDto>> GetListAsync(MediaDescriptorGetListInput input)
        {
            var totalCount = await MediaDescriptorRepository.GetCountAsync();
            var entites = await MediaDescriptorRepository.GetListAsync();

            var dtos = ObjectMapper.Map<List<MediaDescriptor>, List<MediaDescriptorGetListDto>>(entites);
            
            return new PagedResultDto<MediaDescriptorGetListDto>(totalCount, dtos);
        }

        public virtual async Task<MediaDescriptorDto> CreateAsync(UploadMediaStreamContent input)
        {
            var newId = GuidGenerator.Create();
            var newEntity = new MediaDescriptor(newId, CurrentTenant.Id, input.Name, input.MimeType, input.ContentLength ?? 0);

            await MediaDescriptorRepository.InsertAsync(newEntity);
            await MediaContainer.SaveAsync(newId.ToString(), input.GetStream());

            return ObjectMapper.Map<MediaDescriptor, MediaDescriptorDto>(newEntity);
        }

        public virtual async Task<MediaDescriptorDto> UpdateAsync(Guid id, UpdateMediaDescriptorDto input)
        {
            var entity = await MediaDescriptorRepository.GetAsync(id);
            
            entity.SetName(input.Name);

            await MediaDescriptorRepository.UpdateAsync(entity);
            
            return ObjectMapper.Map<MediaDescriptor, MediaDescriptorDto>(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await MediaContainer.DeleteAsync(id.ToString());
            await MediaDescriptorRepository.DeleteAsync(id);
        }

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