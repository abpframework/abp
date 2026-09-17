using System;
using System.Collections.Generic;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.Mapperly;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement.Domain.Volo.Abp.TenantManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TenantToTenantConfigurationMapper
    : MapperBase<Tenant, TenantConfiguration>
{
    [MapperIgnoreTarget(nameof(TenantConfiguration.EditionId))]
    [MapperIgnoreTarget(nameof(TenantConfiguration.IsActive))]
    public override partial TenantConfiguration Map(Tenant source);

    [MapperIgnoreTarget(nameof(TenantConfiguration.EditionId))]
    [MapperIgnoreTarget(nameof(TenantConfiguration.IsActive))]
    public override partial void Map(Tenant source, TenantConfiguration destination);

	protected virtual ConnectionStrings Map(List<TenantConnectionString> source)
	{
		var connStrings = new ConnectionStrings();

		if (source == null)
		{
			return connStrings;
		}

		foreach (var connectionString in source)
		{
			connStrings[connectionString.Name] = connectionString.Value;
		}

		return connStrings;
	}
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class TenantToTenantEtoMapper
    : MapperBase<Tenant, TenantEto>
{
    public override partial TenantEto Map(Tenant source);

    public override partial void Map(Tenant source, TenantEto destination);
}