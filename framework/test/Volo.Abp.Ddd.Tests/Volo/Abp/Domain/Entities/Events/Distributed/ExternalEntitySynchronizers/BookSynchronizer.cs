using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers;

public class BookSynchronizer : ExternalEntitySynchronizer<Book, Guid, RemoteBookEto>, ITransientDependency
{
    public BookSynchronizer(IObjectMapper objectMapper, IRepository<Book, Guid> repository) : base(objectMapper, repository)
    {
    }
}