using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

// The root of a hierarchy that already has a filter of its own (ISoftDelete).
public class Announcement : AggregateRoot<Guid>, ISoftDelete
{
    public string Text { get; set; }

    public bool IsDeleted { get; set; }
}
