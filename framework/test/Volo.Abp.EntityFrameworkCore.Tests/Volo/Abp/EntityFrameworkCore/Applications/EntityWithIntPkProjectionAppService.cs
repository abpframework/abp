using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class EntityWithIntPkProjectionAppService : ReadOnlyAppService<EntityWithIntPk, EntityWithIntPkProjectionDto, int>
{
    public EntityWithIntPkProjectionAppService(IReadOnlyRepository<EntityWithIntPk, int> repository)
        : base(repository)
    {

    }
}
