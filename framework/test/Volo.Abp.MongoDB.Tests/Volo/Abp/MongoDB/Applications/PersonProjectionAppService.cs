using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.MongoDB.Applications;

public class PersonProjectionAppService : ReadOnlyAppService<Person, PersonProjectionDto, Guid>
{
    public PersonProjectionAppService(IReadOnlyRepository<Person, Guid> repository)
        : base(repository)
    {

    }
}
