using Microsoft.AspNetCore.Cors.Infrastructure;
using Volo.Abp.Http;
namespace Microsoft.AspNetCore.Cors;

public static class AbpCorsPolicyBuilderExtensions
{
    public static CorsPolicyBuilder WithAbpExposedHeaders(this CorsPolicyBuilder corsPolicyBuilder)
    {
        return corsPolicyBuilder.WithExposedHeaders(AbpHttpConsts.AbpErrorFormat).WithExposedHeaders(AbpHttpConsts.AbpTenantResolveError);
    }
}
