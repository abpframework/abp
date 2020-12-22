using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Contents
{
    public interface IContentRepository : IBasicRepository<Content, Guid>
    {
        
    }
}