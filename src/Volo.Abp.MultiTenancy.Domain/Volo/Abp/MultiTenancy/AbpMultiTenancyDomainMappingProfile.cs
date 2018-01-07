using AutoMapper;
using Volo.Abp.Data;

namespace Volo.Abp.MultiTenancy
{
    public class AbpMultiTenancyDomainMappingProfile : Profile
    {
        public AbpMultiTenancyDomainMappingProfile()
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
