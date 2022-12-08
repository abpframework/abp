using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers.WithoutEntityVersion;

public class AuthorSynchronizer : ExternalEntitySynchronizer<Author, Guid, RemoteAuthorEto>, ITransientDependency
{
    public AuthorSynchronizer(IObjectMapper objectMapper, IRepository<Author, Guid> repository)
        : base(objectMapper, repository)
    {
    }
}