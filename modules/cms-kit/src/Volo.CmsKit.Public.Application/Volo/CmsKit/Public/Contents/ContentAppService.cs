using System.Threading.Tasks;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Public.Contents
{
    public class ContentAppService : CmsKitAppServiceBase, IContentAppService
    {
        protected readonly IContentRepository ContentRepository;

        public ContentAppService(IContentRepository contentRepository)
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
