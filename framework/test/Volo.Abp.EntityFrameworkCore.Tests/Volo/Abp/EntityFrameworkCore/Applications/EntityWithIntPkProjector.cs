using System.Linq;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class EntityWithIntPkProjector : IQueryableMapper<EntityWithIntPk, EntityWithIntPkProjectionDto>
{
    public IQueryable<EntityWithIntPkProjectionDto> ProjectTo(IQueryable<EntityWithIntPk> source)
    {
        return source.Select(entity => new EntityWithIntPkProjectionDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}
