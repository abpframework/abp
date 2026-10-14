using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class TenantDependentTranslationTestRow : Entity<Guid>
{
    public Guid? OwnerTenantId { get; set; }

    public string Name { get; set; }
}
