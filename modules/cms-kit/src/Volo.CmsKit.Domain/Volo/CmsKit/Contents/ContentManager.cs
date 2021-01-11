using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Domain.Volo.CmsKit.Contents
{
    public class ContentManager : DomainService, IContentManager
    {
        protected IContentRepository ContentRepository { get; }

        public ContentManager(IContentRepository contentRepository)
        {
            ContentRepository = contentRepository;
        }

        public async Task<Content> InsertAsync(Content content, CancellationToken cancellationToken = default)
        {
            if (await ContentRepository.ExistsAsync(content.EntityType, content.EntityId, content.TenantId, cancellationToken))
            {
                throw new ContentAlreadyExistException(content.EntityType, content.EntityId);
            }

            return await ContentRepository.InsertAsync(content);
        }
    }
}
