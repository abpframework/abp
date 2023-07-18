using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain;

public class Category : AggregateRoot<Guid>, ISoftDelete
{
    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}
