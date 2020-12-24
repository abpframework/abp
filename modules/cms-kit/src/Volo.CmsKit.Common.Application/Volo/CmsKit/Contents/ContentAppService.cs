using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Contents
{
    public class ContentAppService : CmsKitAppServiceBase, IContentAppService
    {
        protected readonly IContentRepository ContentRepository;

        public ContentAppService(IContentRepository contentRepository)
        {
            ContentRepository = contentRepository;
        }

        public virtual async Task<ContentDto> GetAsync(string entityType, string entityId)
        {
            var entity = await ContentRepository.FindAsync(entityType, entityId); // Tenant???

            return ObjectMapper.Map<Content, ContentDto>(entity);
        }
    }
}
