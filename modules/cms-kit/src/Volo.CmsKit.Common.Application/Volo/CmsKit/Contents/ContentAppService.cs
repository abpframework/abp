using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.CmsKit.Contents
{
    public class ContentAppService : CmsKitAppServiceBase, IContentAppService
    {
        private readonly IContentRepository _contentRepository;

        public ContentAppService(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<ContentDto> GetAsync(string entityType, string entityId)
        {
            var entity = await _contentRepository.FindAsync(entityType, entityId); // Tenant???

            return ObjectMapper.Map<Content, ContentDto>(entity);
        }
    }
}
