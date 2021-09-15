using System;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.MediaDescriptors;

namespace Volo.CmsKit.Admin.MediaDescriptors
{
    [RequiresGlobalFeature(typeof(MediaFeature))]
    public class MediaDescriptorAdminAppService : CmsKitAdminAppServiceBase, IMediaDescriptorAdminAppService
    {
        protected IBlobContainer<MediaContainer> MediaContainer { get; }
        protected IMediaDescriptorRepository MediaDescriptorRepository { get; }
        protected MediaDescriptorManager MediaDescriptorManager { get; }
        protected IMediaDescriptorDefinitionStore MediaDescriptorDefinitionStore { get; }

        public MediaDescriptorAdminAppService(
            IBlobContainer<MediaContainer> mediaContainer,
            IMediaDescriptorRepository mediaDescriptorRepository,
            MediaDescriptorManager mediaDescriptorManager,
            IMediaDescriptorDefinitionStore mediaDescriptorDefinitionStore)
        {
            MediaContainer = mediaContainer;
            MediaDescriptorRepository = mediaDescriptorRepository;
            MediaDescriptorManager = mediaDescriptorManager;
            MediaDescriptorDefinitionStore = mediaDescriptorDefinitionStore;
        }

        public virtual async Task<MediaDescriptorDto> CreateAsync(string entityType, CreateMediaInputWithStream inputStream)
        {
            var definition = await MediaDescriptorDefinitionStore.GetAsync(entityType);

            /* TODO: Shouldn't CreatePolicies be a dictionary and we check for inputStream.EntityType? */
            await CheckAnyOfPoliciesAsync(definition.CreatePolicies);

            var newEntity = await MediaDescriptorManager.CreateAsync(entityType, inputStream.Name, inputStream.File.ContentType, inputStream.File.ContentLength ?? 0);

            await MediaContainer.SaveAsync(newEntity.Id.ToString(), inputStream.File.GetStream());
            await MediaDescriptorRepository.InsertAsync(newEntity);

            return ObjectMapper.Map<MediaDescriptor, MediaDescriptorDto>(newEntity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var mediaDescriptor = await MediaDescriptorRepository.GetAsync(id);

            var definition = await MediaDescriptorDefinitionStore.GetAsync(mediaDescriptor.EntityType);

            /* TODO: Shouldn't DeletePolicies be a dictionary and we check for inputStream.EntityType? */
            await CheckAnyOfPoliciesAsync(definition.DeletePolicies);

            await MediaContainer.DeleteAsync(id.ToString());
            await MediaDescriptorRepository.DeleteAsync(id);
        }
    }
}
