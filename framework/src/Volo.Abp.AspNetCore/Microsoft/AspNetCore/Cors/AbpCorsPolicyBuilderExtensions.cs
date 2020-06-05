using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Microsoft.AspNetCore.Cors
{
    public static class AbpCorsPolicyBuilderExtensions
    {
        public static CorsPolicyBuilder WithAbpExposedHeaders(this CorsPolicyBuilder corsPolicyBuilder)
        {
            return corsPolicyBuilder.WithExposedHeaders("_AbpErrorFormat");
        }
    }
}