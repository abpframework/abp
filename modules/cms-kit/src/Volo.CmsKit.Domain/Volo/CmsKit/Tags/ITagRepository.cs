using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Tags
{
    public interface ITagRepository : IBasicRepository<Tag, Guid>
    {
    }
}
