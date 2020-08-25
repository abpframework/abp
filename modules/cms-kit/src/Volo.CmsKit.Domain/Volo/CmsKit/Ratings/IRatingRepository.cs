using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Ratings
{
    public interface IRatingRepository : IBasicRepository<Rating, Guid>
    {
        
    }
}