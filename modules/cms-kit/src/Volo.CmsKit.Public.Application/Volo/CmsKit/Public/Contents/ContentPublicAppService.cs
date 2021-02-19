using System.Threading.Tasks;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Public.Contents
{
    public class ContentPublicAppService : CmsKitPublicAppServiceBase, IContentPublicAppService
    {
        protected IContentRepository ContentRepository { get; }

        public ContentPublicAppService(IContentRepository contentRepository)
        {
            ContentRepository = contentRepository;
        }

        public virtual async Task<ContentDto> GetAsync(GetContentInput input)
        {
            var entity = await ContentRepository.GetAsync(
                input.EntityType,
                input.EntityId,
                CurrentTenant.Id);

            return ObjectMapper.Map<Content, ContentDto>(entity);
        }
    }
}
