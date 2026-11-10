using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCacheSoftDeleteRow : Entity<Guid>, ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string Name { get; set; }
}
