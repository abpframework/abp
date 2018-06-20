using AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement
{
    public class AbpTenantManagementDomainMappingProfile : Profile
    {
        public AbpTenantManagementDomainMappingProfile()
        {
            CreateMap<Tenant, TenantInfo>()
                .ForMember(ti => ti.ConnectionStrings, opts =>
                {
                    opts.ResolveUsing(tenant =>
                    {
                        var connStrings = new ConnectionStrings();

                        foreach (var connectionString in tenant.ConnectionStrings)
                        {
                            connStrings[connectionString.Name] = connectionString.Value;
                        }

                        return connStrings;
                    });
                });
        }
    }
}
