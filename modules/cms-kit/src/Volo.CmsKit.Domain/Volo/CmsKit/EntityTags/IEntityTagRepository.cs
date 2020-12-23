using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.EntityTags
{
    public interface IEntityTagRepository : IBasicRepository<EntityTag>
    {
    }
}
